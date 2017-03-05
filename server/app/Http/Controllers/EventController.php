<?php
/**
 * Created by PhpStorm.
 * User: Ty
 * Date: 2017-03-02
 * Time: 4:06 PM
 */

namespace App\Http\Controllers;
use App\EventRecord;
use Illuminate\Http\JsonResponse;
use Illuminate\Http\Request;
use Carbon\Carbon;
use \Response;


class EventController extends Controller
{
    public function __construct()
    {
        $this->middleware('jwt.auth', ['except' => ['createEventRecord','generateEvent']]);
    }

    /**
     * Creates an event record for a blob
     * @param Request $request
     * @return JsonResponse or User
     */
    public function createEventRecord(Request $request)
    {
        // Check that input is correct
        if ($request->exists('blobid')) {
            $ret = $this->verifyUser();
            // Verify that user is valid
            if (is_int($ret)) {
                $user = $ret;
                $blob = $request->input('blobid');
                //Check if owner owns the blob
                $bc = new BlobController();
                $results = $bc->getBlob($blob);
                if ($results->owner_id == $user){
                    $results = EventRecord::where('blob_id', $blob)->first();
                    // Check if blob already has a record
                    if (!$results) {
                        // Create and return RecordID
                        $record = EventRecord::create(array('blob_id' => $blob, 'hungry_timestamp'=> Carbon::now(), 'clean_timestamp' => Carbon::now()));
                        $id = $record->id;
                        EventController::generateEvent($id, 'clean');
                        EventController::generateEvent($id, 'feed');
                        return response()->json(['EventRecordID' => $id], 201);
                    } else {
                        return response()->json(['error' => 'Existing record found for blob'], 400);
                    }
                }
                else{
                    return response()->json(['error' => 'Specified blob does not belong to owner'], 400);
                }

            } else {
                return $ret;
            }
        }
        else{
            return response()->json(['error' => 'Specified blob does not exist'], 400);
        }
    }


    /**
     * Gets the event record based on the event record ID
     * @param $id - the event record ID
     * @return JsonResponse or user
     */
    public function getEventRecord($id){
        $ret = $this->verifyUser();
        // Check that input is correct
        if (is_int($ret)) {
            $user = $ret;
            $record = EventRecord::where('id', $id)->first();
            // Check that a record exists
            if($record) {
                $uc = new UserController();
                $blobs = $uc->getUserBlobs($user);
                // Check that the user does own the blob linked to the event record
                $ownsblob = false;
                foreach ($blobs as $blob) {
                    if ($blob->id == $record->blob_id) {
                        $ownsblob = true;
                    }
                }
                if ($ownsblob) {
                    // return the record
                    return $record;
                } else {
                    return response()->json(['error' => 'Specified record does not belong to owner'], 400);
                }
            } else{
                return response()->json(['error' => 'Record does not exist'], 400);
            }
        } else {
            return $ret;
        }
    }

    public function generateTimestamp(){
        $period = 6;
        $now = Carbon::now();
        $maxtime = Carbon::create($now->year,$now->month,$now->day,$now->hour+$period,$now->minute,$now->second);
        $randomDate = Carbon::createFromTimestamp(rand($now->timestamp, $maxtime->timestamp));
        return $randomDate;
    }

    public function generateEvent($id, $type){
        $period = 12;
        $now = Carbon::now();
        $maxtime = Carbon::create($now->year,$now->month,$now->day,$now->hour+$period,$now->minute,$now->second);
        $randomDate = Carbon::createFromTimestamp(rand($now->timestamp, $maxtime->timestamp));
        $record = EventRecord::where('id', $id)->first();
        if ($type == 'feed'){
            $record->hungry_timestamp = $randomDate;
        }
        elseif($type == 'clean'){
            $record->clean_timestamp = $randomDate;
        }
        $record->save();
    }

}
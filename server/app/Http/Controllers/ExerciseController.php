<?php
/**
 * Created by PhpStorm.
 * User: Ty
 * Date: 2017-02-23
 * Time: 6:18 PM
 */

namespace App\Http\Controllers;

use App\ExerciseRecord;
use Illuminate\Http\JsonResponse;
use Illuminate\Http\Request;
use \DateTime;
use Carbon\Carbon;
use \Response;
use Illuminate\Support\Facades\DB;

class ExerciseController extends Controller
{
    public function __construct()
    {
        $this->middleware('jwt.auth', ['except' => ['createExerciseRecord', 'getExerciseRecord', 'updateExerciseRecord']]);
    }

    /**
     * Creates an exercise record if the user does not already own one
     * @return JsonResponse
     */
    public function createExerciseRecord()
    {
        $ret = $this->verifyUser();
        if(is_int($ret)) {
            $user = $ret;
            $results = ExerciseRecord::where('owner_id', $user)->first();
            // Check if user already has a record
            if (!$results) {
                // Create and return RecordID
                $record = ExerciseRecord::create(array('owner_id' => $user));
                $id = $record->id;
                return response()->json(['ExerciseRecordID' => $id], 201);
            }
            else{
                return response()->json(['error' => 'Existing record found for user'], 400);
            }
        }
        else{
            return $ret;
        }
    }

    /**
     * Gets the exercise record for a user
     * @param $id
     * @return JsonResponse|ExerciseRecord
     */
    public function getExerciseRecord($id)
    {
        $ret = $this->verifyUser();
        if(is_int($ret)) {
            $record = ExerciseRecord::find($id);
            if ($record) {
                return $record;
            }
            else{
                return response()->json(['error' => 'Invalid ExerciseRecordID'], 400);
            }
        }
        else{
            return $ret;
        }
    }

    /**
     * Updates the exercise record for a user
     * @param Request $request
     * @param $id
     * @return JsonResponse
     */
    public function updateExerciseRecord(Request $request, $id)
    {
        if ($request->exists('distance')) {
            $ret = $this->verifyUser();
            if (is_int($ret)) {
                $user = $ret;
                $record = $this->getExerciseRecord($id);
                // Check that the record exists and that the token owner owns it
                if (is_a($record,'App\ExerciseRecord') and $record->owner_id == $user ) {
                    $distance = $request->distance;

                    // Check if it has been a week and the max_exercise needs to be updated
                    $record->updateRecord();

                    // Update the record
                    $totaldistance = $record->total_exercise;
                    $goaldistance = $record->weekly_goal;
                    $newdistance = $totaldistance + $distance;
                    if($newdistance > $goaldistance){
                        $remaining = 0;
                    }
                    else{
                        $remaining = $goaldistance -$newdistance;
                    }
                    $record->total_exercise = $newdistance;
                    $record->remaining_exercise = $remaining;
                    $record->save();

                    // Update user exercise level
                    $distance = floor($distance);
                    $uc = new UserController();
                    $blobs = $uc->getUserBlobs($user);
                    foreach($blobs as $blob) {
                        $old_exercise = $blob->exercise_level;
                        $new_exercise = $old_exercise + $distance;
                        $blob->exercise_level = $new_exercise;
                        $blob->save();
                    }
                    return Response::make('OK', 200);
                    }
                else{
                    return response()->json(['error' => 'Invalid ExerciseRecordID'], 400);
                }
            } else {
                return $ret;
            }
        } else{
            return response()->json(['error' => 'Did not have all required inputs'], 400);
        }
    }

}
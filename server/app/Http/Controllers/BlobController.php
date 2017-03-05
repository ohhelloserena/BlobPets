<?php

namespace App\Http\Controllers;

use App\Blob;
use Illuminate\Http\Request;
use \DateTime;
use Carbon\Carbon;
use \Response;

use Illuminate\Support\Facades\DB;


class BlobController extends Controller
{
    public function __construct()
	{
		$this->middleware('jwt.auth', ['except' => ['getAllBlobs', 'getBlob', 'getBlobUpdatedAt', 'updateBlob', 'createBlob', 'deleteBlob']]);
	}

    // return a list of all the blobs in the database
    // input:   none
	public function getAllBlobs()
    {
    	$blobs = \App\Blob::all();
        foreach ($blobs as $blob)
        {
            $blob->updateBlob();
        }
    	return $blobs;
    }

    // return the blob with the specified blob id
    // return error if the blob does not exist
    // input:   'id': the id of a blob
    public function getBlob($id)
    {
        $blob = \App\Blob::find($id);
        if (empty($blob)) {
            return response()->json(['error' => 'Blob ID invalid'], 400);
        }        
        $blob->updateBlob();
        return $blob;
    }

    // update the specified blob's name (and type?, maybe not...)
    // return error if the blob does not exist
    // input:   'id': the id of a blob
    //          'name': new name for the blob
    public function updateBlobName(Request $request, $id) {
        $ret = $this->verifyUser();
        $blob = BlobController::getBlob($id);
        if(is_int($ret)){
            $user = $ret;
            if (!empty($blob) and $user == $blob->owner_id){

                $blob->updateBlob();

                $new_name = $request->input('name', $blob->name);
                // $new_type = $request->input('type', $blob->type);

                $blob->name = $new_name;
                // $blob->type = $new_type;

                $blob->save();

                return Response::make('OK', 200);
            }
            else{
                return response()->json(['error' => 'Blob ID invalid'], 400);
            }
        }
        else {
            return ret;
        }
    }

    /**
     * Creates a new blob if owner does not already have maxNumBlobs
     * @param Request $request
     * @return \Illuminate\Http\JsonResponse
     */
    public function createBlob(Request $request)
    {
        $maxNumBlobs = 4;
        $expected = array('name', 'type', 'color', 'token');
        // check for correct number of inputs
        if ($request->exists($expected)) {
            $blobName = $request->input('name');
            $blobType = $request->input('type');
            $blobColor = $request->input('color');

            // check if owner is valid
            $ret = $this->verifyUser();
            if (is_int($ret)) {
                $user = $ret;
                $numBlobs = DB::table('blobs')->where('owner_id', $user)->count();

                // check that user does not have the max number of blobs
                if ($numBlobs<$maxNumBlobs){

                    //check if inputs are valid
                    $supportedTypes = array('A', 'B', 'C');
                    if (in_array($blobType, $supportedTypes)) {
                        $blob = Blob::create(array('name' => $blobName, 'type' => 'type ' .$blobType, 'owner_id' => $user, 'color' => $blobColor));
                        $id = $blob->id;

                        // return blob id
                        return response()->json(['blobID' => $id], 201);
                    } else{
                        return response()->json(['error' => 'Invalid inputs'], 400);
                    }
                }
                else{
                    return response()->json(['error' => 'User has max number of blobs'], 400);
                }
            }
            else {
                return $ret;
            }
        } else {
            return response()->json(['error' => 'Did not have all required inputs'], 400);
        }

    }

    /**
     * Deletes a blob if blob health = 0
     * @param $id - the id of the blob to delete
     * @return \Illuminate\Http\JsonResponse
     */
    public function deleteBlob($id){
        $ret = $this->verifyUser();
        if(is_int($ret)) {
            $user = $ret;
            // Check if blob exists and that it belongs to user
            $results = Blob::where('id', $id)->where('owner_id',$user)->first();
            if(!empty($results)) {
                // Check if blob health = 0
                if ($results->health_level == 0){
                    // Delete blob
                    Blob::destroy($id);
                    return Response::make('OK', 200);
                }
                else{
                    return response()->json(['error' => 'Blob is alive'], 400);
                }
            }
            else{
                return response()->json(['error' => 'Invalid blobID'], 400);
            }
        }
        else{
            return $ret;
        }

    }


}

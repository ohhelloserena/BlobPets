<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use \DateTime;
use Carbon\Carbon;
use \Response;

class BlobController extends Controller
{
    public function __construct()
	{
		$this->middleware('jwt.auth', ['except' => ['getAllBlobs', 'getBlob', 'getBlobUpdatedAt', 'updateBlob']]);
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
    public function updateBlob(Request $request, $id) {
        $blob = BlobController::getBlob($id);
        if (empty($blob)) {
            return response()->json(['error' => 'Blob ID invalid'], 400);
        }
        $blob->updateBlob();

        $new_name = $request->input('name', $blob->name);
        // $new_type = $request->input('type', $blob->type);

        $blob->name = $new_name;
        // $blob->type = $new_type;

        $blob->save();

        return Response::make('OK', 200);
    }


    // do not use this one yet
    public function createBlob(Request $request)
    {

        $token = $request->input('token');
        $associatedUser = JWTAuth::toUser($token);
        $user = \App\User::all()->find($associatedUser->id);

        return $blob;

        try {
            // verify the credentials and create a token for the user
            if (! $token = JWTAuth::attempt($credentials)) {
                return response()->json(['error' => 'invalid_credentials'], 401);
            }
        } catch (JWTException $e) {
            // something went wrong
            return response()->json(['error' => 'could_not_create_token'], 500);
        }

        // if no errors are encountered we can return a JWT
        return response()->json(compact('token'));

        
    }

}

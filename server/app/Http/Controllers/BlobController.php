<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;

class BlobController extends Controller
{
    public function __construct()
	{
		$this->middleware('jwt.auth', ['except' => ['getAllBlobs']]);
	}

	public function getAllBlobs(Request $request)
    {
    	$blobs = \App\Blob::all();
    	// return $users;
    	//return $token;
    	// return $associatedUser;
    	return $blobs;
    	
        // TODO: show users
    }
}

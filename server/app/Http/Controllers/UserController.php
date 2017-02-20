<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;

use AppHttpRequests;
use AppHttpControllersController;
use Tymon\JWTAuth\Facades\JWTAuth;
use Tymon\JWTAuthExceptions\JWTException;

class UserController extends Controller
{
    //

	public function __construct()
	{
		$this->middleware('jwt.auth', ['except' => ['authenticate', 'getAllUsers']]);
	}

	public function getAllUsers(Request $request)
    {
    	$users = \App\User::with("blobs")->get();
    	// return $users;
    	//return $token;
    	// return $associatedUser;
    	return $users;
    	
        // TODO: show users
    }

    public function getTokenOwner(Request $request)
    {
    	$token = $request->input('token');

    	$associatedUser = JWTAuth::toUser($token);

    	$users = \App\User::all();
    	// return $users;
    	//return $token;
    	// return $associatedUser;
    	return $users->find($associatedUser->id);
    	
        // TODO: show users
    }



    public function authenticate(Request $request)
    {
        $credentials = $request->only('email', 'password');

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
        // return JWTAuth::toUser($token);
    }

}

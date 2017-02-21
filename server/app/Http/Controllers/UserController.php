<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;

use AppHttpRequests;
use AppHttpControllersController;
use Tymon\JWTAuth\Facades\JWTAuth;
use Tymon\JWTAuthExceptions\JWTException;

class UserController extends Controller
{

	public function __construct()
	{
		$this->middleware('jwt.auth', ['except' => ['authenticate', 'getAllUsers', 'getUser', 'getUserBlobs']]);
	}

    // return a list of all the users in the database
    // include the list of blobs associated with each user
    // input:   none
	public function getAllUsers()
    {
    	$users = \App\User::with("blobs")->get();
    	return $users;
    }

    // return the user with the specified user id
    // include the list of blobs associated with the user
    // return error if the user does not exist
    // input:   'id': the id of a user
    public function getUser($id) {
        $user = \App\User::with("blobs")->find($id);
        if (empty($user)) {
            return response()->json(['error' => 'User ID invalid'], 400);
        }
        return $user;
    }

    // return the list of blobs associated with the user with user id
    // return error if the user does not exist
    // input:   'id': the id of a user
    public function getUserBlobs($id) {
        $user = \App\User::find($id);
        if (empty($user)) {
            return response()->json(['error' => 'User ID invalid'], 400);
        }
        return $user->blobs;
    }

    // return a token that the user will include for subsequent API calls that require user authentication
    // return error if the credentials provided is invalid or if other errors are encountered
    // input:   'email': the email of the user
    //          'password': the password of the user
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
    }

    // debug function
    // return the user associated with the token
    // input:   'token': the token of a user
    public function getTokenOwner(Request $request)
    {
        $token = $request->input('token');

        $associatedUser = JWTAuth::toUser($token);

        $users = \App\User::all();
        return $users->find($associatedUser->id);
    }

}

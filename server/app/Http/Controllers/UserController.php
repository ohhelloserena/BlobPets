<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;

use AppHttpRequests;
use AppHttpControllersController;
use Tymon\JWTAuth\Facades\JWTAuth;
use Tymon\JWTAuthExceptions\JWTException;
use \App\User;
use \Hash;
use \Response;

class UserController extends Controller
{

	public function __construct()
	{
		$this->middleware('jwt.auth', ['except' => ['authenticate', 'getAllUsers', 'getUser', 'getUserBlobs', 'createUser']]);
	}

    // return a list of all the users in the database
    // include the list of blobs associated with each user
    // input:   none
	public function getAllUsers()
    {
    	$users = User::with("blobs")->get();
    	return $users;
    }

    // return the user with the specified user id
    // include the list of blobs associated with the user
    // return error if the user does not exist
    // input:   'id': the id of a user
    public function getUser($id) {
        $user = User::with("blobs")->find($id);
        if (empty($user)) {
            return response()->json(['error' => 'User ID invalid'], 400);
        }
        return $user;
    }

    // return the list of blobs associated with the user with user id
    // return error if the user does not exist
    // input:   'id': the id of a user
    public function getUserBlobs($id) {
        $user = User::find($id);
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
        $associatedUser = JWTAuth::toUser($token);
        $id = $associatedUser->id;
        return response()->json(compact('token', 'id'));
    }

    // Create a new user with the specified name, email, and password. Return the new user's id if successfully created
    // Return error if some input fields are missing, or the email is already taken by another user
    // input:   'name': the name of the user
    //          'email': the email of the user
    //          'password': the password of the user
    public function createUser(Request $request) {

        $required = array('name', 'email', 'password');
        if ($request->exists($required)) {
            $userEmail = $request->input('email');
            $userWithEmail = User::where('email', $userEmail)->first();
            $userlat = $request->input('lat');
            $userlong = $request->input('long');

            if (empty($userWithEmail)) {
                $credentials = $request->only('name', 'email', 'password');
                if (!empty($userlat) and !empty($userlong)){
                    $credentials['latitude'] = $userlat;
                    $credentials['longitude'] = $userlong;
                }
                $credentials['password'] = Hash::make($credentials['password']);
                $newUser = User::create($credentials);
                return Response::make($newUser->id, 201);
            }
            else {
                return response()->json(['error' => 'Email address has already been taken'], 400);
            }

        }
        else {
            return response()->json(['error' => 'Missing required input fields'], 400);
        }
        
    }

    // Update an existing user
    // Return error if the user id specified does not exist in the database
    // Return error if the user is trying to modify another user's profile
    // Return 'OK' if the operation was successful
    // input:   'name': the new name for the user profile
    //          'password': the new password for the user profile
    //          'token': the token associated with the logged in user
    public function updateUser($id, Request $request) {

        $token = $request->input('token');
        $newName = $request->input('name');
        $newPassword = $request->input('password');
        $newlat = $request->input('lat');
        $newlong = $request->input('long');

        $associatedUser = JWTAuth::toUser($token);
        $user = User::find($associatedUser->id);
        if ($checkExist = User::find($id)) {
            if ($user->id != $id) {
                return Response::make('Unauthorized', 401);
            }
        }
        else {
            return response()->json(['error' => 'User with specified id does not exist'], 400);
        }
        
        if (!empty($newName)) {
            $user->name = $newName;
        }
        if (!empty($newPassword)) {
            $user->password = Hash::make($newPassword);
        }
        if (!empty($newlat) and !empty($newlong)){
            $user->latitude = $newlat;
            $user->longitude = $newlong;
        }
        $user->save();

        return Response::make('OK', 200);

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

<?php

use Illuminate\Http\Request;

/*
|--------------------------------------------------------------------------
| API Routes
|--------------------------------------------------------------------------
|
| Here is where you can register API routes for your application. These
| routes are loaded by the RouteServiceProvider within a group which
| is assigned the "api" middleware group. Enjoy building your API!
|
*/

Route::middleware('auth:api')->get('/user', function (Request $request) {
    return $request->user();
});

Route::group(['prefix' => 'users'], function()
{
	Route::get('/', 'UserController@getAllUsers');	// return list of all users
	Route::get('/{id}', 'UserController@getUser');	// return user with user id
	Route::get('/{id}/blobs', 'UserController@getUserBlobs');	// return list of all blobs owned by user with user id

    Route::post('authenticate', 'UserController@authenticate');	// authenticate user with email and password, and return a token

	Route::get('getTokenOwner', 'UserController@getTokenOwner');	// debug function
	
});

Route::group(['prefix' => 'blobs'], function()
{
	Route::get('/', 'BlobController@getAllBlobs');	// return list of all blobs
    Route::post('/', 'BlobController@createBlob'); // creates a blob
	Route::get('/{id}', 'BlobController@getBlob');	// return blob with blob id
	Route::put('/{id}', 'BlobController@updateBlob');	// update blob's name
    Route::delete('/{id}', 'BlobController@deleteBlob'); // deletes a blob
});

/*
|--------------------------------------------------------------------------
| Some examples for using the API
|--------------------------------------------------------------------------
| - GET serverAddress.com/api/users
|		return a list of all users
| - GET serverAddress.com/api/users/1
|		return the user with user id 1
| - POST serverAddress.com/api/users/authenticate?email=mail@email.com&password=password
|		return a token for user with email 'mail@email.com' and 
|		with password 'password', given that this is in the database
|
| - POST serverAddress.com/api/blobs/?token=<token>&name=blob&type=A&color=purple
|       creates a blob if user has less than 4 blobs and returns the blob id
| - GET serverAddress.com/api/blobs/
|       gets information about all blobs
|
| - PUT serverAddress.com/api/blobs/1?name=Blobby
|		change the name of blob with id 1 to 'Blobby'
| - GET serverAddress.com/api/blobs/1
|       gets blob info for blob with id 1
| - DELETE serverAddress.com/api/blobs/1?token=<token>
|       deletes blob with id 1 if health = 0
|
*/
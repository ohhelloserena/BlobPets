<?php

use Illuminate\Http\Request;
use Illuminate\Support\Facades\Route;

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


    Route::post('/', 'UserController@createUser');	// create a user
    Route::post('authenticate', 'UserController@authenticate');	// authenticate user with email and password, and return a token

    Route::put('/{id}', 'UserController@updateUser');	// update an existing user

	Route::get('getTokenOwner', 'UserController@getTokenOwner');	// debug function
	
});

Route::group(['prefix' => 'blobs'], function()
{
	Route::get('/', 'BlobController@getAllBlobs');	// return list of all blobs
    Route::post('/', 'BlobController@createBlob'); // creates a blob
	Route::get('/{id}', 'BlobController@getBlob');	// return blob with blob id
	Route::put('/{id}', 'BlobController@updateBlob');	// update blob's name or level attributes
    Route::delete('/{id}', 'BlobController@deleteBlob'); // deletes a blob
    Route::post('/breed', 'BlobController@breedBlob'); // creates a blob through breeding
});

Route::group(['prefix' => 'exercises'], function()
{
    Route::post('/', 'ExerciseController@createExerciseRecord'); // Creates a new exercise record if there is none

    Route::get('/{id}', 'ExerciseController@getExerciseRecord'); // Returns the exercise record with record id
    Route::put('/{id}', 'ExerciseController@updateExerciseRecord'); // Updates the exercise record
});

Route::group(['prefix' => 'battles'], function(){
    Route::post('/', 'BattleController@createBattleRecord');
    Route::get('/', 'BattleController@getBattleRecords');
    Route::get('/{id}', 'BattleController@getBattleRecord');
});

//Route::group(['prefix' => 'events'], function(){
//    Route::post('/', 'EventController@createEventRecord');
//    Route::get('/{id}', 'EventController@getEventRecord');
//});

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
| - PATCH serverAddress.com/api/blobs/1?name=Blobby
|		change the name of blob with id 1 to 'Blobby'
| - GET serverAddress.com/api/blobs/1
|       gets blob info for blob with id 1
| - DELETE serverAddress.com/api/blobs/1?token=<token>
|       deletes blob with id 1 if health = 0
|
|   POST serverAddress.com/api/exercises
|       creates an exercise record for the user if it doesn't already exist
|   GET serverAddress.com/api/exercises/{id}?token=<token>
|       Gets an exercise record for a user
|   PUT serverAddress.com/api/exercises/{id}?distance=5&token=<token>
|       Updates and exercise record for a user with distance walked since last update
|
|   POST serverAddress.com/api/battles?blob1=<>&blob2=<>
|       creates a battle record
|   GET serverAddress.com/api/battles?user=<>
|   GET serverAddress.com/api/battles?blob=<>
|   GET serverAddress.com/api/battles
|       Gets the battles associated with the user, blob or all records
|   GET serverAddress.com/api/battles/{id}
|       Get a specific battle record
|
|   POST serverAddress.com/api/events
|       creates an event record for a blob
|   GET serverAddress.com/api/events/{id}
|       gets the event record if the user owns the blob associated with the event record
*/
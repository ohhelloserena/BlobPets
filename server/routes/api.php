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

Route::get('getAllUsers', function() {

	$users = \App\User::all();

	return $users;

});

// Route::post('authenticate', 'AuthenticateController@authenticate');
// Route::get('authenticate', 'AuthenticateController@index');
// Route::get('authenticate', 'AuthenticateController@index');




Route::group(['prefix' => 'user'], function()
{
    Route::post('authenticate', 'UserController@authenticate');
	Route::get('getTokenOwner', 'UserController@getTokenOwner');
	Route::get('getAllUsers', 'UserController@getAllUsers');
});

Route::group(['prefix' => 'blob'], function()
{
	Route::get('getAllBlobs', 'BlobController@getAllBlobs');
});
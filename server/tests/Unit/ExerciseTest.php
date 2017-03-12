<?php
/**
 * Created by PhpStorm.
 * User: Ty
 * Date: 2017-03-10
 * Time: 10:21 PM
 */

namespace tests\Unit;

use App\Http\Controllers\BlobController;
use Carbon\Carbon;
use Tests\TestCase;
use App\Http\Controllers\ExerciseController;

class ExerciseTest extends TestCase
{
//$response = $this->call('POST', '/api/users/authenticate', array('password' => 'secret'));
//|   POST serverAddress.com/api/exercises
//|       creates an exercise record for the user if it doesn't already exist
//|   GET serverAddress.com/api/exercises/{id}?token=<token>
//|       Gets an exercise record for a user
//|   PUT serverAddress.com/api/exercises/{id}?distance=5&token=<token>
//|       Updates and exercise record for a user with distance walked since last update


    public $user_token = '';

    protected function setUp(){
        parent::setUp();
        $response = $this->call('POST', '/api/users/authenticate', array('email' => 'adnan@scotch.io', 'password' => 'secret'));
        $response_json = json_decode($response->getContent());
        $this->user_token = $response_json->token;
        $this->artisan("migrate:refresh");
        $this->artisan("db:seed");
    }

    public function tearDown()
    {
        parent::tearDown();
    }
    public function testCreateRecord(){
        $token = (string)$this->user_token;

        //Create Record with no token
        $response = $this->call('POST', '/api/exercises', array());
        $response_json = json_decode($response->getContent());
        $this->assertEquals(401, $response->getStatusCode());
        $error = $response_json->error;
        $this->assertEquals('The token could not be parsed from the request', $error);

        //Create record with token
        $this->refreshApplication();
        $response = $this->call('POST','/api/exercises', [], [], [], ['HTTP_Authorization' => 'Bearer' . $token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(201, $response->getStatusCode());
        $this->assertEquals($response_json->ExerciseRecordID, 3);

        //Try to create record again
        $this->refreshApplication();
        $response = $this->call('POST','/api/exercises', [], [], [], ['HTTP_Authorization' => 'Bearer' . $token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(400, $response->getStatusCode());
        $this->assertEquals('Existing record found for user', $response_json->error);
    }

    public function testGetRecord(){
        //Get Record For User
        $this->refreshApplication();
        $response = $this->call('GET','/api/exercises/1', [], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(200, $response->getStatusCode());
        $this->assertEquals(1, $response_json->id);
        $this->assertEquals(1, $response_json->owner_id);
        $this->assertEquals(0, $response_json->total_exercise);
        $this->assertEquals(5, $response_json->weekly_goal);
        $this->assertEquals(5, $response_json->remaining_exercise);

        //Get record for another user
        $this->refreshApplication();
        $response = $this->call('GET','/api/exercises/2', [], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(200, $response->getStatusCode());
        $this->assertEquals(2, $response_json->id);
        $this->assertEquals(2, $response_json->owner_id);
        $this->assertEquals(0, $response_json->total_exercise);
        $this->assertEquals(5, $response_json->weekly_goal);
        $this->assertEquals(5, $response_json->remaining_exercise);

        //Get record with invalid RecordID
        $this->refreshApplication();
        $response = $this->call('GET','/api/exercises/3', [], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(400, $response->getStatusCode());
        $this->assertEquals('Invalid ExerciseRecordID', $response_json->error);

    }

    public function testUpdateExerciseRecord(){

        // Update record without valid inputs
        $this->refreshApplication();
        $response = $this->call('PUT','/api/exercises/1', [], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(400, $response->getStatusCode());
        $this->assertEquals('Did not have all required inputs', $response_json->error);

        // Update record for another user
        $this->refreshApplication();
        $response = $this->call('PUT','/api/exercises/2', ['distance'=>1], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(400, $response->getStatusCode());
        $this->assertEquals('Invalid ExerciseRecordID', $response_json->error);

        // Update record that does not exist
        $this->refreshApplication();
        $response = $this->call('PUT','/api/exercises/6', ['distance'=>1], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(400, $response->getStatusCode());
        $this->assertEquals('Invalid ExerciseRecordID', $response_json->error);

        // Update record
        $this->refreshApplication();
        $this->call('POST','/api/exercises', [], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);

        $this->refreshApplication();
        $response = $this->call('PUT','/api/exercises/3', ['distance'=>1], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $this->assertEquals(200, $response->getStatusCode());

        $this->refreshApplication();
        $response = $this->call('GET','/api/exercises/3', [], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(3, $response_json->id);
        $this->assertEquals(4, $response_json->owner_id);
        $this->assertEquals(1, $response_json->total_exercise);
        $this->assertEquals(5, $response_json->weekly_goal);
        $this->assertEquals(4, $response_json->remaining_exercise);
    }


    public function testUpdateRecord(){
        $this->refreshApplication();
        $today = Carbon::now();
        $this->call('POST','/api/exercises', [], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);

        // Update record while on current week
        $this->call('GET','/api/exercises/3', [], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $ec = new ExerciseController();
        $record = $ec->getExerciseRecord(3);
        $record->updateRecord($today);

        $bc = new BlobController();
        $blob_record = $bc->getBlob(4);
        $this->assertEquals(60, $blob_record->exercise_level);

        $this->refreshApplication();
        $response = $this->call('GET','/api/exercises/3', [], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(0, $response_json->total_exercise);
        $this->assertEquals(5, $response_json->weekly_goal);

        // Get record for next week after meeting max
        $this->call('PUT','/api/exercises/3', ['distance'=>5], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $next_week = Carbon::createFromDate($today->year,$today->month,$today->day+7);
        $record = $ec->getExerciseRecord(3);
        $record->updateRecord($next_week);

        $this->refreshApplication();
        $response = $this->call('GET','/api/exercises/3', [], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(5, $response_json->total_exercise);
        $this->assertEquals(10, $response_json->weekly_goal);

        $blob_record = $bc->getBlob(4);
        $this->assertEquals(65, $blob_record->exercise_level);

        // Get record for next week without meeting max
        $next_week = Carbon::createFromDate($next_week->year,$next_week->month,$next_week->day+7);
        $record = $ec->getExerciseRecord(3);
        $record->updateRecord($next_week);

        $this->refreshApplication();
        $response = $this->call('GET','/api/exercises/3', [], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(5, $response_json->total_exercise);
        $this->assertEquals(15, $response_json->weekly_goal);

        $blob_record = $bc->getBlob(4);
        $this->assertEquals(55, $blob_record->exercise_level);
    }

}
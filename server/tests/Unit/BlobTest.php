<?php
/**
 * Created by PhpStorm.
 * User: Ty
 * Date: 2017-03-16
 * Time: 7:05 PM
 */

namespace tests\Unit;

use Tests\TestCase;
use App\Http\Controllers\BlobController;


class BlobTest extends TestCase
{
    public $user_token = '';

    protected function setUp()
    {
        parent::setUp();
        $response = $this->call('POST', '/api/users/authenticate', array('email' => 'ryanchenkie@gmail.com', 'password' => 'secret'));
        $response_json = json_decode($response->getContent());
        $this->user_token = $response_json->token;
        $this->artisan("migrate:refresh");
        $this->artisan("db:seed");
    }

    public function tearDown()
    {
        parent::tearDown();
    }

    public function testCreateBlob(){
        // Missing multiple inputs
        $this->refreshApplication();
        $response = $this->call('POST','/api/blobs', [], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(400, $response->getStatusCode());
        $this->assertEquals('Did not have all required inputs', $response_json->error);

        // Missing an input
        $this->refreshApplication();
        $response = $this->call('POST','/api/blobs', ['name'=>'testy', 'type'=>'A'], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(400, $response->getStatusCode());
        $this->assertEquals('Did not have all required inputs', $response_json->error);

        // Missing token
        $this->refreshApplication();
        $response = $this->call('POST','/api/blobs', ['name'=>'testy', 'type'=>'A', 'color'=>'yellow'], [], [], []);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(401, $response->getStatusCode());
        $this->assertEquals('The token could not be parsed from the request', $response_json->error);

        // Invalid inputs, type is invalid
        $this->refreshApplication();
        $response = $this->call('POST','/api/blobs', ['name'=>'testy', 'type'=>'X', 'color'=>'yellow'], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(400, $response->getStatusCode());
        $this->assertEquals('Invalid inputs', $response_json->error);

        // Create blob
        $this->refreshApplication();
        $response = $this->call('POST','/api/blobs', ['name'=>'testy', 'type'=>'A', 'color'=>'yellow'], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(201, $response->getStatusCode());
        $this->assertEquals(6, $response_json->blobID);

        $bc = new BlobController();
        $blob = $bc->getBlob(6);
        $this->assertEquals(1, $blob->owner_id);
        $this->assertEquals('testy', $blob->name);
        $this->assertEquals('type A', $blob->type);
        $this->assertEquals('yellow', $blob->color);

        // Hit max number of blobs
        $this->refreshApplication();
        $response = $this->call('POST','/api/blobs', ['name'=>'testy', 'type'=>'A', 'color'=>'yellow'], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(201, $response->getStatusCode());
        $this->assertEquals(7, $response_json->blobID);

        $this->refreshApplication();
        $response = $this->call('POST','/api/blobs', ['name'=>'testy2', 'type'=>'A', 'color'=>'yellow'], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(400, $response->getStatusCode());
        $this->assertEquals('User has max number of blobs', $response_json->error);

    }

    public function testDeleteBlob(){
        // Missing Token
        $this->refreshApplication();
        $response = $this->call('DELETE','/api/blobs/50', [], [], [], []);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(401, $response->getStatusCode());
        $this->assertEquals('The token could not be parsed from the request', $response_json->error);

        // Invalid BlobID, blob doesn't exist
        $this->refreshApplication();
        $response = $this->call('DELETE','/api/blobs/50', [], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(400, $response->getStatusCode());
        $this->assertEquals('Invalid blobID', $response_json->error);

        // Invalid BlobID, blob doesn't belong to the user
        $this->refreshApplication();
        $response = $this->call('DELETE','/api/blobs/2', [], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(400, $response->getStatusCode());
        $this->assertEquals('Invalid blobID', $response_json->error);

        // Blob is alive
        $this->refreshApplication();
        $response = $this->call('DELETE','/api/blobs/1', [], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(400, $response->getStatusCode());
        $this->assertEquals('Blob is alive', $response_json->error);

        // Blob exists and is dead
        $bc = new BlobController();
        $blob = $bc->getBlob(1);
        $blob->alive = false;
        $blob->save();

        $this->refreshApplication();
        $response = $this->call('DELETE','/api/blobs/1', [], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        json_decode($response->getContent());
        $this->assertEquals(200, $response->getStatusCode());
    }

    public function testCalculateLevel(){
        $bc = new BlobController();

        $this->assertEquals(10, $bc->calculateLevel(10, 100, 100, 100));
        $this->assertEquals(5, $bc->calculateLevel(10, 100, 0, 0));
        $this->assertEquals(3, $bc->calculateLevel(10, 0, 100, 0));
        $this->assertEquals(2, $bc->calculateLevel(10, 0, 0, 100));
        $this->assertEquals(0, $bc->calculateLevel(10, 0, 0, 0));
    }

    public function testGetTopBlobs(){
        $bc = new BlobController();
        $blob = $bc->getBlob(2);
        $blob->level=60;
        $blob->save();

        $blob = $bc->getBlob(4);
        $blob->level=100;
        $blob->save();

        $blob = $bc->getBlob(5);
        $blob->level=20;
        $blob->save();

        // 5 blobs in system

        $this->refreshApplication();
        $response = $this->call('GET','/api/blobs/getTopBlobs', [], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(200, $response->getStatusCode());

        $records = $response_json;
        $this->assertEquals(5, sizeof($records));
        $top1 = $records[0];
        $this->assertEquals(4, $top1->id);
        $top2 = $records[1];
        $this->assertEquals(2, $top2->id);
        $top3 = $records[2];
        $this->assertEquals(5, $top3->id);
        $top4 = $records[3];
        $this->assertEquals(1, $top4->id);
        $top5 = $records[4];
        $this->assertEquals(3, $top5->id);

        $this->refreshApplication();
        $response = $this->call('POST','/api/blobs', ['name'=>'testy', 'type'=>'A', 'color'=>'yellow'], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(201, $response->getStatusCode());
        $this->assertEquals(6, $response_json->blobID);

        // More than 5 blobs in the system
        $blob = $bc->getBlob(6);
        $blob->level=10;
        $blob->save();

        $this->refreshApplication();
        $response = $this->call('GET','/api/blobs/getTopBlobs', [], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(200, $response->getStatusCode());

        $records = $response_json;
        $this->assertEquals(5, sizeof($records));
        $top1 = $records[0];
        $this->assertEquals(4, $top1->id);
        $top2 = $records[1];
        $this->assertEquals(2, $top2->id);
        $top3 = $records[2];
        $this->assertEquals(5, $top3->id);
        $top4 = $records[3];
        $this->assertEquals(6, $top4->id);
        $top5 = $records[4];
        $this->assertEquals(1, $top5->id);

        // Less than 5 blobs in system
        $blob = $bc->getBlob(5);
        $blob->alive=false;
        $blob->save();

        $this->refreshApplication();
        $response = $this->call('DELETE','/api/blobs/5', [], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        json_decode($response->getContent());
        $this->assertEquals(200, $response->getStatusCode());

        $blob = $bc->getBlob(1);
        $blob->alive=false;
        $blob->save();

        $this->refreshApplication();
        $response = $this->call('DELETE','/api/blobs/1', [], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        json_decode($response->getContent());
        $this->assertEquals(200, $response->getStatusCode());

        $this->refreshApplication();
        $response = $this->call('GET','/api/blobs/getTopBlobs', [], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(200, $response->getStatusCode());

        $records = $response_json;
        $this->assertEquals(4, sizeof($records));
        $top1 = $records[0];
        $this->assertEquals(4, $top1->id);
        $top2 = $records[1];
        $this->assertEquals(2, $top2->id);
        $top3 = $records[2];
        $this->assertEquals(6, $top3->id);
        $top4 = $records[3];
        $this->assertEquals(3, $top4->id);
    }

    public function testGetBlob(){
        $response = $this->call('GET','/api/blobs/1');
        $response_json = json_decode($response->getContent());
        $this->assertEquals(200, $response->getStatusCode());
    }

    public function testGetBlobInvalidId(){
        $response = $this->call('GET','/api/blobs/-1');
        $response_json = json_decode($response->getContent());
        $this->assertEquals(400, $response->getStatusCode());
    }

//| - PATCH serverAddress.com/api/blobs/1?name=Blobby
//|     change the name of blob with id 1 to 'Blobby'
    public function testUpdateBlob(){
        $response = $this->call('GET','/api/blobs/1');
        $response_json = json_decode($response->getContent());
        $this->assertEquals(200, $response->getStatusCode());
        $name = $response_json->name;
        $cleanliness_level = $response_json->cleanliness_level;
        $health_level = $response_json->health_level;

        // Failing example
        $this->refreshApplication();
        $response = $this->call('PUT','/api/blobs/1', [], [], [], []);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(400, $response->getStatusCode());
        $this->assertEquals('token_not_provided', $response_json->error);


        $response = $this->call('POST', '/api/users/authenticate', array('email' => 'chris@scotch.io', 'password' => 'secret'));
        $response_json = json_decode($response->getContent());
        $this->assertEquals(200, $response->getStatusCode());
        $user_token = $response_json->token;
        $user_id = $response_json->id;

        $this->refreshApplication();
        $response = $this->call('PUT','/api/blobs/1', [], [], [], ['HTTP_Authorization' => 'Bearer' . $user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(401, $response->getStatusCode());
        $this->assertEquals('Unauthorized action', $response_json->error);


        // Valid example
        $this->refreshApplication();
        $response = $this->call('PUT','/api/blobs/1', ['name'=>'testy'], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(200, $response->getStatusCode());

        $bc = new BlobController();
        $blob = $bc->getBlob(1);
        $this->assertEquals('testy', $blob->name);


        $this->refreshApplication();
        $response = $this->call('PUT','/api/blobs/1', ['cleanliness_level'=> $cleanliness_level + 10], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(200, $response->getStatusCode());

        $bc = new BlobController();
        $blob = $bc->getBlob(1);
        $this->assertEquals($cleanliness_level + 10, $blob->cleanliness_level);

        $this->refreshApplication();
        $response = $this->call('PUT','/api/blobs/1', ['cleanliness_level'=> $cleanliness_level + 10], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(403, $response->getStatusCode());
        $this->assertEquals('Request rejected', $response_json->error);


        $this->refreshApplication();
        $response = $this->call('PUT','/api/blobs/1', ['health_level'=> $health_level + 10], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(200, $response->getStatusCode());

        $bc = new BlobController();
        $blob = $bc->getBlob(1);
        $this->assertEquals($health_level + 10, $blob->health_level);

        $this->refreshApplication();
        $response = $this->call('PUT','/api/blobs/1', ['health_level'=> $health_level + 10], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(403, $response->getStatusCode());
        $this->assertEquals('Request rejected', $response_json->error);


        $this->artisan("migrate:refresh");
        $this->artisan("db:seed");
        $this->refreshApplication();
        $response = $this->call('PUT','/api/blobs/1', ['name'=>'testy', 'cleanliness_level'=> $cleanliness_level + 10], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(200, $response->getStatusCode());

        $bc = new BlobController();
        $blob = $bc->getBlob(1);
        $this->assertEquals('testy', $blob->name);
        $this->assertEquals($cleanliness_level + 10, $blob->cleanliness_level);


        $this->artisan("migrate:refresh");
        $this->artisan("db:seed");
        $this->refreshApplication();
        $response = $this->call('PUT','/api/blobs/1', ['cleanliness_level'=> $cleanliness_level + 10, 'health_level'=> $health_level + 10], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(200, $response->getStatusCode());

        $bc = new BlobController();
        $blob = $bc->getBlob(1);
        $this->assertEquals($cleanliness_level + 10, $blob->cleanliness_level);
        $this->assertEquals($health_level + 10, $blob->health_level);


        $this->artisan("migrate:refresh");
        $this->artisan("db:seed");
        $this->refreshApplication();
        $response = $this->call('PUT','/api/blobs/1', ['name'=>'testy', 'health_level'=> $health_level + 10], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(200, $response->getStatusCode());

        $bc = new BlobController();
        $blob = $bc->getBlob(1);
        $this->assertEquals('testy', $blob->name);
        $this->assertEquals($health_level + 10, $blob->health_level);


        $this->artisan("migrate:refresh");
        $this->artisan("db:seed");
        $this->refreshApplication();
        $response = $this->call('PUT','/api/blobs/1', ['name'=>'testy', 'cleanliness_level'=> $cleanliness_level + 10, 'health_level'=> $health_level + 10], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(200, $response->getStatusCode());

        $bc = new BlobController();
        $blob = $bc->getBlob(1);
        $this->assertEquals('testy', $blob->name);
        $this->assertEquals($cleanliness_level + 10, $blob->cleanliness_level);
        $this->assertEquals($health_level + 10, $blob->health_level);

        // invalid id
        $response = $this->call('PUT','/api/blobs/-1', ['name'=>'testy', 'cleanliness_level'=> $cleanliness_level + 10, 'health_level'=> $health_level + 10], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(400, $response->getStatusCode());        
        $this->assertEquals('Blob ID invalid', $response_json->error);

        // invalid token
        $response = $this->call('PUT','/api/blobs/1', [], [], [], []);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(401, $response->getStatusCode());
        $this->assertEquals('The token could not be parsed from the request', $response_json->error);
    }

    public function testBreedBlob(){

        $response = $this->call('POST','/api/blobs/breed', ['id1' => 1, 'id2' => 5], [], [], ['HTTP_Authorization' => 'Bearer' . "token"]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(401, $response->getStatusCode());
        $this->assertEquals('The token could not be parsed from the request', $response_json->error);

        $response = $this->call('POST','/api/blobs/breed', ['id1' => 1], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(400, $response->getStatusCode());
        $this->assertEquals('Did not have all required inputs', $response_json->error);

        $this->refreshApplication();
        $response = $this->call('POST','/api/blobs/breed', ['id1' => -1, 'id2' => 5], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(400, $response->getStatusCode());
        $this->assertEquals('Invalid blob id', $response_json->error);

        $this->artisan("migrate:refresh");
        $this->artisan("db:seed");
        $response = $this->call('POST','/api/users', ['name' => 'maxBlobUser', 'email' => 'maxBlobUser@gmail.com', 'password' => 'secret'], [], [], []);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(201, $response->getStatusCode());

        $response = $this->call('POST', '/api/users/authenticate', array('email' => 'maxBlobUser@gmail.com', 'password' => 'secret'));
        $response_json = json_decode($response->getContent());
        $this->assertEquals(200, $response->getStatusCode());
        $user_token = $response_json->token;
        $user_id = $response_json->id;

        $response = $this->call('POST','/api/blobs', ['name'=>'maxBlobUserBlob1', 'type'=>'A', 'color'=>'yellow'], [], [], ['HTTP_Authorization' => 'Bearer' . $user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(201, $response->getStatusCode());
        $blobIdOne = $response_json->blobID;

        $response = $this->call('POST','/api/blobs', ['name'=>'maxBlobUserBlob2', 'type'=>'A', 'color'=>'yellow'], [], [], ['HTTP_Authorization' => 'Bearer' . $user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(201, $response->getStatusCode());
        $blobIdTwo = $response_json->blobID;

        $response = $this->call('POST','/api/blobs/breed', ['id1' => $blobIdOne, 'id2' => $blobIdTwo], [], [], ['HTTP_Authorization' => 'Bearer' . $user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals('User has max number of blobs', $response_json->error);
        $this->assertEquals(201, $response->getStatusCode());

        $response = $this->call('POST','/api/blobs/breed', ['id1' => $blobIdOne, 'id2' => $blobIdTwo], [], [], ['HTTP_Authorization' => 'Bearer' . $user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(201, $response->getStatusCode());

        $response = $this->call('POST','/api/blobs/breed', ['id1' => $blobIdOne, 'id2' => $blobIdTwo], [], [], ['HTTP_Authorization' => 'Bearer' . $user_token]);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(400, $response->getStatusCode());
        $this->assertEquals('User has max number of blobs', $response_json->error);
    }



}
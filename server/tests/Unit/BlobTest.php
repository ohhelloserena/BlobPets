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

//| - PATCH serverAddress.com/api/blobs/1?name=Blobby
//|		change the name of blob with id 1 to 'Blobby'
    public function testUpdateBlob(){
        // Failing example
        $this->refreshApplication();
        $response = $this->call('PUT','/api/blobs/1', [], [], [], []);
        $response_json = json_decode($response->getContent());
        $this->assertEquals(401, $response->getStatusCode());
        $this->assertEquals('The token could not be parsed from the request', $response_json->error);

        // Valid example
        $this->refreshApplication();
        $response = $this->call('PUT','/api/blobs/1', ['name'=>'testy'], [], [], ['HTTP_Authorization' => 'Bearer' . $this->user_token]);
        json_decode($response->getContent());
        $this->assertEquals(200, $response->getStatusCode());

        $bc = new BlobController();
        $blob = $bc->getBlob(1);
        $this->assertEquals('testy', $blob->name);

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

}
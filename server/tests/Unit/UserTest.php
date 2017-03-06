<?php

namespace Tests\Unit;

use Tests\TestCase;
use Illuminate\Foundation\Testing\DatabaseMigrations;
use Illuminate\Foundation\Testing\DatabaseTransactions;

class UserTest extends TestCase
{
    /**
     * A basic test example.
     *
     * @return void
     */
    public function testExample()
    {
        $this->assertTrue(true);
    }

    public function testUserLoginForExistingUser()
    {
    	$response = $this->call('POST', '/api/users/authenticate', array('email' => 'ryanchenkie@gmail.com', 'password' => 'secret'));
        $response_json = json_decode($response->getContent());
        $this->assertEquals(200, $response->getStatusCode());
        $user_token = $response_json->token;
        $user_id = $response_json->id;
        $this->assertEquals(1, $user_id);

        $response = $this->call('POST', '/api/users/authenticate', array('email' => 'chris@scotch.io', 'password' => 'secret'));
        $response_json = json_decode($response->getContent());
        $this->assertEquals(200, $response->getStatusCode());
        $user_token = $response_json->token;
        $user_id = $response_json->id;
        $this->assertEquals(2, $user_id);

        $response = $this->call('POST', '/api/users/authenticate', array('email' => 'holly@scotch.io', 'password' => 'secret'));
        $response_json = json_decode($response->getContent());
        $this->assertEquals(200, $response->getStatusCode());
        $user_token = $response_json->token;
        $user_id = $response_json->id;
        $this->assertEquals(3, $user_id);

        $response = $this->call('POST', '/api/users/authenticate', array('email' => 'adnan@scotch.io', 'password' => 'secret'));
        $response_json = json_decode($response->getContent());
        $this->assertEquals(200, $response->getStatusCode());
        $user_token = $response_json->token;
        $user_id = $response_json->id;
        $this->assertEquals(4, $user_id);
    }

    public function testUserLoginForNonExistingUser()
    {
    	$response = $this->call('POST', '/api/users/authenticate', array('email' => 'some_email@gmail.com', 'password' => 'blah'));
        $response_json = json_decode($response->getContent());
        $this->assertEquals(401, $response->getStatusCode());
        $error = $response_json->error;
        $this->assertEquals($error, 'invalid_credentials');
    }

}

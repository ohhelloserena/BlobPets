<?php

use Illuminate\Support\Facades\Schema;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Database\Migrations\Migration;

class CreateBlobsTable extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('blobs', function (Blueprint $table) {
            $table->increments('id');
            $table->string('name');
            $table->string('type');
            $table->boolean('alive')->default(true);
            $table->integer('level')->unsigned()->default(1);
            $table->integer('exercise_level')->default(60);
            $table->integer('cleanliness_level')->default(60);
            $table->integer('health_level')->default(60);
            $table->integer('owner_id')->unsigned();
            $table->timestamps();
        });

        Schema::table('blobs', function (Blueprint $table) {
            $table->foreign('owner_id')->references('id')->on('users')->onDelete('cascade');
        });
    }

    /**
     * Reverse the migrations.
     *
     * @return void
     */
    public function down()
    {
        Schema::dropIfExists('blobs');
    }
}

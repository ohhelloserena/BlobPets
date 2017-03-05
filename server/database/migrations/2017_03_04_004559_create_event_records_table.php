<?php

use Illuminate\Support\Facades\Schema;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Database\Migrations\Migration;

class CreateEventRecordsTable extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('event_records', function (Blueprint $table) {
            $table->increments('id');
            $table->integer('blob_id')->unsigned();
            $table->dateTime('hungry_timestamp');
            $table->dateTime('clean_timestamp');
            $table->timestamps();
        });

        Schema::table('event_records', function (Blueprint $table) {
            $table->foreign('blob_id')->references('id')->on('blobs')->onDelete('cascade');
        });
    }

    /**
     * Reverse the migrations.
     *
     * @return void
     */
    public function down()
    {
        Schema::dropIfExists('event_records');
    }
}

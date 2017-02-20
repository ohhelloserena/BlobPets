<?php

use Illuminate\Database\Seeder;
use Illuminate\Database\Eloquent\Model;

use App\Blob;

use Faker\Factory as Faker;

class BlobSeed extends Seeder
{
	/**
	 * Run the database seeds.
	 *
	 * @return void
	 */
	public function run()
	{
		$blobs = array(
			['name' => 'Blob 1', 'type' => 'type A', 'owner_id' => 1],
			['name' => 'Blob 2', 'type' => 'type A', 'owner_id' => 2],
			['name' => 'Blob 3', 'type' => 'type B', 'owner_id' => 3],
			['name' => 'Blob 4', 'type' => 'type B', 'owner_id' => 4],
			['name' => 'Blob 5', 'type' => 'type C', 'owner_id' => 1],
		);

		foreach ($blobs as $blob)
        {
            Blob::create($blob);
        }
        
	}
}

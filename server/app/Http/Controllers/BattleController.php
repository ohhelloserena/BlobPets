<?php
/**
 * Created by PhpStorm.
 * User: Ty
 * Date: 2017-03-08
 * Time: 10:20 PM
 */

namespace App\Http\Controllers;

use Illuminate\Database\Eloquent\Collection;
use Illuminate\Http\JsonResponse;
use Illuminate\Http\Request;
use App\BattleRecord;

class BattleController extends Controller
{
    public function __construct()
    {
        $this->middleware('jwt.auth', ['except' => ['createBattleRecord','getBattleRecords', 'getBattleRecord']]);
    }

    /**
     * Creates a battle record
     * @param Request $request
     * @return JsonResponse
     */
    public function createBattleRecord(Request $request){
        $required = array('blob1', 'blob2');
        if ($request->exists($required)){
            //Verify that token in valid
            $ret = $this->verifyUser();
            if(is_int($ret)) {
                $user = $ret;
                $blob1_id = $request->input('blob1');
                $blob2_id = $request->input('blob2');
                $bc = new BlobController();
                $blob1 = $bc->getBlob($blob1_id);
                $blob2 = $bc->getBlob($blob2_id);
                //Verify that only one of the blobs is owned by them
                if ($blob1->owner_id == $user xor $blob2->owner_id == $user){
                    //Compute winner
                    $winner = $this->determineWinner($blob1,$blob2);
                    if ($winner->id == $blob1_id){
                        $loser = $blob2;
                        }
                    else{
                        $loser = $blob1;
                    }
                    $this->rewardWinner($winner);
                    $this->updateWinner($winner->owner_id);
                    $this->punishLoser($loser);

                    //Return battle record
                    $record = BattleRecord::create(array('loserBlobID' => $loser->id, 'winnerBlobID' => $winner->id));
                    $id = $record->id;
                    return response()->json(['BattleRecordID' => $id], 201);
                }
                else{
                    return response()->json(['error' => 'Invalid blobs, either own both or none'], 400);
                }
            }
            else{
                return $ret;
            }
        }
        else{
            return response()->json(['error' => 'Missing required input fields'], 400);
        }

    }

    /**
     * Gets a list of battle records for a specific user, a specific blob or all existing records
     * @param Request $request
     * @return Collection|static[]
     */
    public function getBattleRecords(Request $request){
        $blob = $request->input('blob');
        $user = $request->input('user');
        if (!empty($blob)){
            // Gets the records that include a blob
            $records = BattleRecord::where('winnerBlobID', $blob)->orWhere('loserBlobID', $blob)->get();
            return $records;
        }
        elseif (!empty($user)){
            //Returns all battle records related to a user
            $uc = new UserController();
            $blobs = $uc->getUserBlobs($user);
            $records = new Collection();
            foreach ($blobs as $blob) {
                $blobid = $blob->id;
                $record = BattleRecord::where('winnerBlobID', $blobid)->orWhere('loserBlobID', $blobid)->get();
                $records->push($record);
            }
            return $records;
        }
        else{
            //Returns all records
            $records = BattleRecord::all();
            return $records;
        }
    }

    /**
     * Gets a specific battle record
     * @param $id - the battleRecordID
     * @return JsonResponse
     */
    public function getBattleRecord($id){
        $record = BattleRecord::where('id', $id)->first();
        // Check that a record exists
        if($record) {
            // return the record
            return $record;
        } else{
            return response()->json(['error' => 'Record does not exist'], 400);
        }
    }
    
    /**
     * Determines the winner of the battle
     * @param $blob1
     * @param $blob2
     * @return int(the id of the user)
     */
    public function determineWinner($blob1, $blob2){
        // Calculate values
        $exercise = $blob1->exercise_level;
        $health = $blob1->health_level;
        $clean = $blob1->cleanliness_level;
        $level = $blob1->level;

        $blob1_value = $level * ((0.005*$exercise) + (0.003*$health) +(0.002*$clean));
        $exercise = $blob2->exercise_level;
        $health = $blob2->health_level;
        $clean = $blob2->cleanliness_level;
        $level = $blob2->level;
        $blob2_value = $level * ((0.005*$exercise) + (0.003*$health) +(0.002*$clean));

        // Determine winner
        if ($blob1_value > $blob2_value){
            $winner = $blob1;
        }
        else if ($blob1_value < $blob2_value){
            $winner = $blob2;
        }
        else{
            // The targeted blob wins if it is a tie
            $winner = $blob2;
        }

        // Return winner blob
        return $winner;
    }

    /**
     * Update user record for winner
     * @param $winner - the owner_id of the winning blob
     */
    public function updateWinner($winner){
        $uc = new UserController();
        $winner_record = $uc->getUser($winner);
        $winner_record->battles_won = $winner_record->battles_won + 1;
        $winner_record->save();
    }

    //TODO determine how much to punish by or do we decrement the level
    /**
     * Update losing blob with a punishment
     * @param $blob
     */
    public function punishLoser($blob){
        $punish = 1;
        $prevBlobHealth = $blob->health_level;
        $prevBlobClean = $blob->cleanliness_level;
        $prevBlobExercise = $blob->exercise_level;

        $blob->health_level =  $prevBlobHealth - $punish;
        $blob->cleanliness_level = $prevBlobClean - $punish;
        $blob->exercise_level = $prevBlobExercise - $punish;

        $blob->save();
    }

    //TODO determine how to reward winner, do we have some sort of experience thing or do we just increase the level
    /**
     * Updates the winning blob with some reward
     * @param $blob - the blob that won the battle
     */
    public function rewardWinner($blob){
        $prevBlobLevel = $blob->level;

        $blob->level = $prevBlobLevel + 1;

        $blob->save();
    }
}
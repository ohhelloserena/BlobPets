<?php

namespace App;

use App\Http\Controllers\UserController;
use Illuminate\Database\Eloquent\Model;
use \DateTime;
use Carbon\Carbon;

class ExerciseRecord extends Model
{
    //
    protected $guarded = array();

    /**
     * Updates the max_exercise record of the user if it is a new week
     * Also decrements the exercise_level of the user if they haven't met their weekly total
     */
    public function updateRecord(){
        $km_per_week = 5;
        $now = Carbon::now();
        $last_update = $this->updated_at;
        $diff = $last_update->diffInDays($now);
        $day_of_week = $last_update->dayOfWeek;
        if ($day_of_week == 0){
            // this is sunday
            $days_left = $day_of_week;
        }
        else{
            //rest of the week
            $days_left = 7 - $day_of_week;
        }
        // New week
        if($days_left < $diff){
            $prevmax = $this->max_exercise;
            $current_total = $this->total_exercise;

            // Decrement exercise level
            if ($current_total < $prevmax){
                $user_id = $this->owner_id;
                $uc = new UserController();
                $blobs = $uc->getUserBlobs($user_id);
                foreach($blobs as $blob){
                    $old_health = $blob->exercise_level;
                    $new_health = $old_health - 10;
                    $blob->exercise_level = $new_health;
                    $blob->save();
                }
            }

            // Update Max
            $newmax = $prevmax + $km_per_week;
            $this->max_exercise = $newmax;
            $this->save;
        }
        else{
            return;
        }
    }

}

<?php

namespace App;

use Illuminate\Database\Eloquent\Model;
use \DateTime;
use Carbon\Carbon;

class Blob extends Model
{
    //
    // public function updatedAt()
    // {
    //     return $this->updated_at;
    // }

    public function updateBlob() {

        $old_time = $this->updated_at->getTimestamp();
        $now = Carbon::now()->getTimestamp();

        // time difference in second
        $timeDifference = $now - $old_time;
        // $year = $timeDifference->y;
        // $month = $timeDifference->m;
        // $day = $timeDifference->d;
        // $hour = $timeDifference->h;
        // $minute = $timeDifference->i;
        // $second = $timeDifference->s;

        // update only if at least 1 minute has passed
        // currently decrease exercise_level, cleanliness_level, and health_level by 1 for each minute passed
        if ($timeDifference >= 60) {
            $minutesPassedSinceUpdate = round($timeDifference / 60);

            $old_exercise_level = $this->exercise_level;
            $old_cleanliness_level = $this->cleanliness_level;
            $old_health_level = $this->health_level;

            $new_exercise_level = $old_exercise_level - $minutesPassedSinceUpdate;
            $new_cleanliness_level = $old_cleanliness_level - $minutesPassedSinceUpdate;
            $new_health_level = $old_health_level - $minutesPassedSinceUpdate;

            $new_blob_levels = array('exercise_level' => $new_exercise_level, 'cleanliness_level' => $new_cleanliness_level, 'health_level' => $new_health_level);
            $this->exercise_level = $new_exercise_level;
            $this->cleanliness_level = $new_cleanliness_level;
            $this->health_level = $new_health_level;
            $this->save();

        }

        


    }

}

import SimpleJSON;
import UnityEngine.UI;
#pragma strict

var url = "http://104.131.144.86/api/users/";

var data;

var user : UI.Text;
var wins : UI.Text;
var blobs : UI.Text;

var b0 : UI.Text;
var b1 : UI.Text;
var b2 : UI.Text;
var b3 : UI.Text;

var userId;
var username = "-1";
var numWins = "-1";
var numBlobs = -1;

var blobName0 = "Locked!";
var blobName1 = "Locked!";
var blobName2 = "Locked!";
var blobName3 = "Locked!";


var blobId0 = -1;
var blobId1 = -1;
var blobId2 = -1;
var blobId3 = -1;


function Start () {

    var www: WWW = new WWW(url);

// wait for request to complete
    yield www;

// check for errors
    if (www.error == null) {
        data = www.text;
        Debug.Log("data: " + data);

        ParseJson();

        Debug.Log("userId: " + userId);
        Debug.Log("username: " + username);
        Debug.Log("numWins: " + numWins);
        Debug.Log("numBlobs: " + numBlobs);

        Debug.Log("blobName0: " + blobName0);
        Debug.Log("blobId0: " + blobId0);

        Debug.Log("blobName1: " + blobName1);
        Debug.Log("blobId1: " + blobId1);

        Debug.Log("blobName2: " + blobName2);
        Debug.Log("blobId2: " + blobId2);

        Debug.Log("blobName3: " + blobName3);
        Debug.Log("blobId3: " + blobId3);

        SetUsernameText();
        SetNumWinsText();
        SetNumBlobsText();
        SetBlobNames();

    } else {
        Debug.Log("WWW Error: " + www.error);
    }
}

function ParseJson() {
    var N = JSON.Parse(data);

// get user info

    userId = N["id"].AsInt;
    username = N["name"].Value;
    numWins = N["battles_won"].Value;
    numBlobs = N["blobs"].Count;

// get blobs info 

    blobName0 = N["blobs"][0]["name"].Value;
    blobId0 = N["blobs"][0]["id"].AsInt;

    blobName1 = N["blobs"][1]["name"].Value;
    blobId1 = N["blobs"][1]["id"].AsInt;
}


// set username text
function SetUsernameText() {
    user.text = username;
}

// set numWon text
function SetNumWinsText() {
    wins.text = numWins;
}

function SetNumBlobsText() {
    blobs.text = numBlobs.ToString();
}

function SetBlobNames() {
    b0.text = blobName0;
    b1.text = blobName1;
    b2.text = blobName2;
    b3.text = blobName3;
}


function Update () {

}




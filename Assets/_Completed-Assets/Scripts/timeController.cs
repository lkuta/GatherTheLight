using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Controls the countdown timer and win conditions:
public class timeController : MonoBehaviour {

    // global variables needed for timers:
    public float countdownTimerDelay;
    public float countdownTimerStartTime;
    public int i = 0;
    public float timeRemaining;
    public float lightCal;
  
    // Global variables needed for results calculations:
    public GameObject resultsCanvas;
    public GameObject continueText;
    public Text timer;
    public bool endGame = false;

    public float lightTotal;
    public float timeTotal;
    public float exitTotal;

    // Timer code given by professor:
    public void CountdownTimerReset(float delayInSeconds)
    {
        countdownTimerDelay = delayInSeconds;
        countdownTimerStartTime = Time.time;
    }

    public float CountdownTimerSecondsRemaining()
    {
        float elapsedSeconds = Time.time - countdownTimerStartTime;
        float timeLeft = elapsedSeconds - countdownTimerDelay;
        return timeLeft;
    }

    void Start()
    {
        // On Start, hide the results screen:
        resultsCanvas = GameObject.Find("Canvas/winCanvas");
        resultsCanvas.SetActive(false);

        //Also, hide the specific results text:
        continueText = GameObject.Find("Canvas/exitFoundLabel");
        continueText.SetActive(false);
    }

    // Starts the timer and displays the remaining time:
    // Also displays the game over screen and calculates the results
    void Update() { 
    
    // Press the Escape key in the build to end the game at any time:
    if (Input.GetKey("escape")) {
        Application.Quit();
    }

    // Get the current scene and default the timer script to blank:
    Scene scene = SceneManager.GetActiveScene();
        timer = GameObject.Find("Canvas/countDownText").GetComponent<Text>();
        timer.text = " ";

        // If Level 1, start the timer:
        if (string.Compare(scene.name, "Level 1") == 0) {
            if (i == 0) {
                CountdownTimerReset(200.0f);
                i++;
            }
            // Stop the clock if the game ends:
            if (endGame == false) {
                // Calculate time remaining and display it:
                timeRemaining = -(CountdownTimerSecondsRemaining());
                Text timer2 = GameObject.Find("Canvas/countDownText").GetComponent<Text>();
                timer2.text = timeRemaining.ToString();
            }

            // Getting variables while ending the game:
            CompletePlayerController player = GameObject.Find("Player").GetComponent<CompletePlayerController>();
            endGameResults(player);
        }
    }

    // Determines the overall results of the player's performance
    public void endGameResults(CompletePlayerController player){
        if (timeRemaining < 0 && player.count != 12 && player.hitFinish <= 0) {
            // Complete loss
            endGame = true;
            resultsCanvas.SetActive(true);
            Text resultsDisp = GameObject.Find("Canvas/winCanvas/resultsDisp").GetComponent<Text>();
            resultsDisp.text = "You were crushed as the tunnels collasped and did not gather all the light. You lost!";
            lightTotal = displayLightGathered();
            timeTotal = displayTimeResults();
            exitTotal = displayExitResults(player.hitFinish);
            displayTotalScore(lightTotal, timeTotal, exitTotal);

        } else if (timeRemaining < 0 && player.count != 12 && player.hitFinish >= 1) {
            // Hit the finish, but didn't get all of the light orbs
            resultsCanvas.SetActive(true);
            endGame = true;
            Text resultsDisp = GameObject.Find("Canvas/winCanvas/resultsDisp").GetComponent<Text>();
            resultsDisp.text = "You escaped this tunnel network, but did not gather all of the light. Great job!";
            Text clear = continueText.GetComponent<Text>();
            clear.text = " ";
            continueText.SetActive(false);
            lightTotal = displayLightGathered();
            timeTotal = displayTimeResults();
            exitTotal = displayExitResults(player.hitFinish);
            displayTotalScore(lightTotal, timeTotal, exitTotal);

        } else if (timeRemaining < 0 && player.count == 12 && player.hitFinish <= 0) {
            // Got all of the light orbs, but didn't get to the finish
            resultsCanvas.SetActive(true);
            endGame = true;
            Text resultsDisp = GameObject.Find("Canvas/winCanvas/resultsDisp").GetComponent<Text>();
            resultsDisp.text = "You gathered all of the light, but in the process were crushed as the tunnels collasped." +
                "Make sure you are touch the exit after gathering all of the light.";
            lightTotal = displayLightGathered();
            timeTotal = displayTimeResults();
            exitTotal = displayExitResults(player.hitFinish);
            displayTotalScore(lightTotal, timeTotal, exitTotal);

        } else if (timeRemaining >= 0 && player.count == 12 && player.hitFinish >= 1) {
            // Complete win - did everything
            resultsCanvas.SetActive(true);
            endGame = true;
            Text resultsDisp = GameObject.Find("Canvas/winCanvas/resultsDisp").GetComponent<Text>();
            resultsDisp.text = "You escaped the tunnels with all of the light. Amazing!!";
            lightTotal = displayLightGathered();
            timeTotal = displayTimeResults();
            exitTotal = displayExitResults(player.hitFinish);
            displayTotalScore(lightTotal, timeTotal, exitTotal);

        } else if (timeRemaining >= 0 && player.count != 12 && player.hitFinish >= 1) {
            // Small pop-up - tell them to either keep going and collect the rest of the orbs or hit button to end game
            continueText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Space)) {
                endGame = true;
                resultsCanvas.SetActive(true);
                continueText.SetActive(false);
                Text clear = continueText.GetComponent<Text>();
                clear.text = " ";
                Text resultsDisp = GameObject.Find("Canvas/winCanvas/resultsDisp").GetComponent<Text>();
                resultsDisp.text = "You escaped this tunnel network, but did not gather all of the light. Great job!";
                lightTotal = displayLightGathered();
                timeTotal = displayTimeResults();
                exitTotal = displayExitResults(player.hitFinish);
                displayTotalScore(lightTotal, timeTotal, exitTotal);
            }
        }
    }

    // Calculates light gathered and score based off of that:
    public float displayLightGathered()
    {
        // Get the needed objects:
        Text lightDisp = GameObject.Find("Canvas/winCanvas/lightCountDisp").GetComponent<Text>();
        Text lightScore = GameObject.Find("Canvas/winCanvas/lightCountScore").GetComponent<Text>();
        CompletePlayerController player2 = GameObject.Find("Player").GetComponent<CompletePlayerController>();
        
        // Show number of lights collected:
        lightDisp.text = player2.count.ToString();
        
        // Calculate gathered light score:
        lightCal = (player2.count) * 2.50f;
        lightScore.text = lightCal.ToString();

        //Return light score to calculate total score:
        return lightCal;
    }

    // Shows the time remaining and calculates score based off of that:
    public float displayTimeResults()
    {
        Text timeDisp = GameObject.Find("Canvas/winCanvas/timeDisp").GetComponent<Text>();
        Text timeScore = GameObject.Find("Canvas/winCanvas/timeScore").GetComponent<Text>();

        // If the player used all of the given time:
        if (timeRemaining <= 0 ) {
            timeDisp.text = "0";
            timeScore.text = "0";
            timer.text = " ";
            return 0f;

        // If the player finishe before time ran out:
        } else {
            int displayTime = (int)timeRemaining;
            timeDisp.text = displayTime.ToString();
            float timeCal = Mathf.RoundToInt((1.50f)*(displayTime));
            timeScore.text = timeCal.ToString();
            timer.text = " ";
            return timeCal;
        }
    }

    // Shows whether or not the person hit exit at the end of time:
    public float displayExitResults(int finished)
    {
        Text exitDisp = GameObject.Find("Canvas/winCanvas/exitDisp").GetComponent<Text>();
        Text exitScore = GameObject.Find("Canvas/winCanvas/exitScore").GetComponent<Text>();

        // If they reached the finish:
        if (finished >= 1) {
            exitDisp.text = "Yes!";
            exitScore.text = "30";
            return 30f;

        // If they didn't reach the finish:
        } else {
            exitDisp.text = "No...";
            exitScore.text = "0";
            return 0f;
        }
    }

    // Shows the final total score
    public void displayTotalScore(float light, float time, float exit)
    {
        Text totDisp = GameObject.Find("Canvas/winCanvas/scoreDisp").GetComponent<Text>();
        float total = light + time + exit;
        totDisp.text = ((int)total).ToString();
    }

    public void OnResetButtonClicked (){
        // Clear any values that need to be cleared:
        lightTotal = 0;
        timeTotal = 0;
        exitTotal = 0;
        i = 0;
        endGame = false;

        // Clear all of the text on the results screen:
        Text resultsDisp = GameObject.Find("Canvas/winCanvas/resultsDisp").GetComponent<Text>();
        Text lightDisp = GameObject.Find("Canvas/winCanvas/lightCountDisp").GetComponent<Text>();
        Text lightScore = GameObject.Find("Canvas/winCanvas/lightCountScore").GetComponent<Text>();
        Text timeDisp = GameObject.Find("Canvas/winCanvas/timeDisp").GetComponent<Text>();
        Text timeScore = GameObject.Find("Canvas/winCanvas/timeScore").GetComponent<Text>();
        Text exitDisp = GameObject.Find("Canvas/winCanvas/exitDisp").GetComponent<Text>();
        Text exitScore = GameObject.Find("Canvas/winCanvas/exitScore").GetComponent<Text>();
        Text totDisp = GameObject.Find("Canvas/winCanvas/scoreDisp").GetComponent<Text>();

        resultsDisp.text = " ";
        lightDisp.text = " ";
        lightScore.text = " ";
        timeDisp.text = " ";
        timeScore.text = " ";
        exitDisp.text = " ";
        exitScore.text = " ";
        totDisp.text = " ";

        // Hide the results screen:
        resultsCanvas.SetActive(false);

        // Switch scene to the instructions screen:
        SceneManager.LoadScene("Instruction Screen");

    }
   
}

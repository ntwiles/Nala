int input;
int chosenNumber;
int maxNumGuesses;
int currentNumGuesses;
bool playing;
string feedbackMessage;

chosenNumber = 57;
maxNumGuesses = 10;
currentNumGuesses = maxNumGuesses;
playing = true;

echoline "I've picked a random number! Try to guess it.";
echoline "Press any enter to start.";
read;

while (playing)
{
	clear;

	if (currentNumGuesses != maxNumGuesses)
	{
		echo "You guessed ";
		echoline input;
		echoline "";
		echoline feedbackMessage;
		echoline "";
		echo "Try again! ";
		echo currentNumGuesses;
		echoline " more tries.";
	}

	echoline "Please enter a number between 0 and 100";
	echoline "";

	read input;
	currentNumGuesses = currentNumGuesses - 1;

	if (input < chosenNumber)
	{
		feedbackMessage = "Not quite! Too low.";
	}

	if (input > chosenNumber)
	{
		feedbackMessage = "Wrong, sorry! Too high.";
	}

	if (input == chosenNumber)
	{
		echoline "You guessed it! Good job.";
		echoline "Press any key to quit.";
		playing = false;
	}

	if (currentNumGuesses == 0)
	{
		clear;
		echoline "You're out of tries!";
		echoline "Press any key to quit.";
		playing = false;
	}
}

read;



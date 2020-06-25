
bool showMessage;
bool dontShowMessage;

showMessage = true;
dontShowMessage = false;

if (true)
{
    echoline "Boolean literals working";
}

if (showMessage)
{
    echoline "Boolean variables working with shorthand.";
}

if (showMessage == true)
{
    echoline "Boolean variables compared against boolean literals working.";
}

if (dontShowMessage)
{
    echoline "Boolean variables NOT working with shorthand.";
}

if (dontShowMessage == true)
{
    echoline "Boolean variables compared against boolean literals NOT working.";
}

if (1 + 1 == 1 + 1)
{
    echoline "Arithmatic comparisons working.";
}

if (1 + 1 == 1 + 1 + 1)
{
    echoline "Multiple operand arithmatic comparisons NOT working.";
}

//if (1 + 1 == false)
//{
    // This should throw an error.
//}


What was done?
I have tried to finish almost all points mentioned in the email so I would as solution is almost completed based on requirements if I correctly understand it.

What wasn't done?
I have added basic unit tests and not all of them.

What would be done with more time?
1. There is a better way of writing console app as mentioned in the below link. This will also cover the logging aspect which is missing in the current solution.
https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-6.0

2. I would include more robust unit tests and try to cover all aspects.
3. Better comments in the code and user friendly error messages.
4. Custom exception handling.
5. Currently I assume we have only XML format as input file and CSV format as output file but if we have more input/output file formats then we could extend it using interfaces or abstract classes and define specific behaviour of read and write implementations.

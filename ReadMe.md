### Summary ###

It was an interesting assignment, and I tried to keep it withing the scope of the time. but I couldn't resist the
temptation arranging the code with clean architecture mind set at leas in namespace and directory level.

### Core deviation for the  changes assignment

I changed the testing framework from **MSTest to xUnit** due to my own comfort and I removed **Newtonsoft and used .Net
builtin System.Text.Json**, it's lightweight and has better performance.

### Time/Scope limitations ###
I avoid doing the list bellow due to time and scope limitation:
1. Change the project to follow the clean architecture.
2. Using distributed cache instead of simple cache or even in the simple cache I could implement it the way that it could handle concurrency.
3. Error handling filters
4. Minimal api
5. Tractable Logging
6. Replacing hardcoded values in appsettings.json with azure app config
7. Using a mapper library to map between objects
8. Smaller commits
9. Changing the return type of **TransactionService.GetHighestPositiveBalanceChange** the current object is confusing and not easy to read.
10. Versioning

#### Note ####
1. I used AI to generate the MemoryCaching service.
2. The ResultModel{T} and its related objects is something that I have been developing and perfecting over the years of working in different companies, it's a small tool but very handy. Therefore, they were copied from old projects, including the custom exceptions. I would wrap all services responses with this model then I wouldn't need to throw exception which is generally bad practice and consumes too much resources.
3. **TransactionService.GetHighestPositiveBalanceChange** was an obvious sliding window over a sorted list for me with led the time complexity to: Time O(n log n) for sort by Date plus O(n) for scanning and a total of **O(n log n)**
4. I got a bit carried a way from the instructions and did the third task before the second.
5. I changed the code they way it was supposed to be for .NET 8, like the Program.cs.
6. I would never write **catch (ArgumentException ex)** it was just for the sake of scope and time.


#### At the end, thank you for the opportunity and an interesting home assignment ####
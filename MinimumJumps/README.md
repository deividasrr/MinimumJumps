# Minimum Jumps 

This .NET Core API practice app that implements a greedy solution for the Minimum Jumps problem.

## Background

In addition, the app has a built-in SQLite database that auto-initializes on the first run of the app.
The database keeps a track record of queries that already ran together with their results. Each API call checks the database if the user input hasn't been encountered before, and if it has, the API responds with the stored result instead of recalculating the result anew.

## Usage

To Use the API, after running, provide the input in the form of:
```
https://localhost:44370/api/minimumjump/getpath?entries=1,2,0,3,0,2,0
```
Where the "entries" GET parameter accepts a list of integers. The response will be an integer representing the shortest jump count possible to reach the end of the array. 
# Semester project SVEN 1 protocol
to document features, the application obsidian has been used\
in order to automate http requests, insomnia has been used

## Unit test strategy: 
low level classes, which do not have dependencies, are part of unit tests, higher level BL classes, who are interdependent, are covered by integration tests.

## Design:
To not violate the open close principle, attributes and reflections have been used to implement http endpoints.\
For the dal the repository pattern has been used. Also, instead of DAO, a class which generates sql statements\
to map c# objects to postgresql tables, has been implemented.\
Implementation of Battle feature:\
The Battle class has a method to register users for battle\
There is also a thread in the battle class which tries to select 2 users from a queue\
If within a limited timeframe two users register, they are matched up and the Battle starts.\
Once the battle is over a log object of the battle results can be fetched from the battle class for each user

## Lessons Learned:
How to use reflections, attributes and repository pattern,\
with unit of work and transactions, how to use opportunistic concurrency for database synchronization

## Unique feature:
Daily login bonus:
If a user logs in on consecutive days they are rewarded in the form of coins\
on the first day 1 coin, on the second and third 2 coins and from the fourth onward 4 coins\
if the users misses a day his streak is reset and starts at one with the next login

## Timeline:
3.10.2021 3h documentation of basic features, setup of repository\
4.10.2021 2h finished feature documentation, created unit test project, first class\
25.11.2021 4h started server implementation using class and method attributes, class to handle connections and parse requests\
26.11.2021 2h added http response class, refactored solution\
27.11.2021 8h added client class, refactored solution, added Nunit project and first test\
28.11.2021 4h designed database structure, wrote sql script\
29.11.2021 8h implemented dal, repository, DAO, dbcontext and UnitofWork class, created first endpoint for user registration\
30.11.2021 2h finished dal\
30.11.2021 2h reworked dao to generic version\
1.12.2021 4h finished dal rework\
1.12.2021 2h refactoring and attributes for models\
4.12.2021 3h added endpoints for sessions and packages + integration tests\
5.12.2021 4h added enpoints up to battle and integration test\
6.12.2021 4h implemented most of the battle logic\
7.12.2021 3h implemented trade endpoint and completed battle logic\
8.12.2021 3h added Unit Tests and unique feature\
17.12.2021 0.5h added some more unit test\
total 58.5h

## Link to repo:
https://github.com/MarkusDuskanich/MTCG

# RubberDuckyEvents sick API
A nice API where you can do the following things.

### Users
- add users
- delete users by id
- edit users
- get all users
- get users by id
- set user attendance
- remove user attendance

### Events
- add events
- delete events
- edit events
- get all events
- get events by age range
- get events by id

## How to use the API
1. Clone the code</br>
`git clone https://github.com/Cournflaeik/Greenfield.git`
2. Open the folder "RubberDuckyEvents.API" </br>
`cd RubberDuckyEvents.API`
3. Update your sqlite database</br>
`dotnet ef database update`
4. Run the damn thing</br>
`dotnet watch run`

## How to test the API
1. If you already cloned the code go to step 2 otherwise clone the code </br>
`git clone https://github.com/Cournflaeik/Greenfield.git`
2. In the root directory run </br>
`dotnet test`
3. See the tests go green 😎

## TODO:
1. Provide extra tests (time constraint ⌚)
2. Provide meaningfull responses (time constraint ⌚)
3. Check the code for consistancy (time constraint ⌚)

## Notes:
1. Tests for getEventsByAgeRange work but no idea how to test specific id's, names, ... for multiple results
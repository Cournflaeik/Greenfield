user{
    id: auto
    name: required
    date_of_birth: required
    mail: required, no duplicate
    attendance: optional
    administrator: optional, default 0
}

actions on user{
    get: retrieve users by name, Retrieve specific user by id, Retrieve users by attendance, Retrieve all administrators by name
    post: add new user
    put: ?? What the bath duck do you use put for?
    patch: update an existing user
    delete: delete a user
}

event{
    id: auto
    name: required
    genre: required
    start_date: required
    duration: optional, default 1 day
    street_name: required
    street_number: required
    city: required
    country: required
    participants_count: default from users who are attending this event 
    age_range: optional
    main_event_id: optional
}
//I think we should add artists to events
//MAINEVENTID in case an event works with periods, you can add 2 events and link them together using the main_event_id
//GENRE should maybe be another array so it'll be cleaner when an event has multiple genres? EXAMPLE BELOW
//event{
//    ...
//    genre{
//        hip hop,
//        future funk, 
//    }
//    ...
//}

actions on event{
    get: retrieve events by name, retrieve events by start_date, retrieve events by city, retrieve specific event by id
    post: add a new event
    put: ?? What the bath duck do you use put for?
    patch: update an existing event
    delete: delete an event
}

user{
    id:;
    name:
    dateOfBirth:
    mail:
    attendance:
    administrator:
    (softdelete (zodat je niet je informatie verliest):
}

actions on user{
    get: alles (uw eigen profiel)
    post: add new user, id can not be post (by generated), email can not be the same --> error, 
    put
    patch
    delete
}

event{
    id:
    name:
    mainEventId (voor als event in 2 periodes wordt opgedeeld, dan worden er 2 'kleine' events gemaakt onder de mainEventId)
    genre:
    startDate:
    duration:
    streetName:
    streetNumber:
    city:
    country:
    participantsCount:
    ageRange(optional):

}

actions on event{
    get: alles event
    post:id can not be post (by generated), ageRange is optional
    put
    patch
    delete
}

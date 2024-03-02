# wundermanthompson
REST API Implementation

I made some asumptions on the buissiness logic.
* On updates the status can not be updated.
* Results are writen by the process but not at the creation or update by the api
* On updates the links are completely removed and created again.

In other topic, I decided to not link the nested properties for performance purposes. So the only endpoint returning link data and results as nested objects in DataJobDTO is the Get by data job id.

Runing the application using the launch.json you will need to add /swagger to the URL to access swagger config. 

# Toggle-Toggl

This is a really simple web api that will help you to start a time entry on Toggl, or to stop the current running task.
I wrote it to trigger my time tracker with a simple DO button or an IFTTT Recipe.

## Actions

* api/toggl/start will start a new time entry
* api/toggl/stop will stop the running time entry

## Params

For the both action, you need to send in the body a json message (application/json) :

```{"togglApiKey":"your-api-key"}```

The api key can be found on bottom of the [profile page](https://www.toggl.com/app/profile).

## General

Thanks to [TogglAPI.net](https://github.com/sochix/TogglAPI.Net)

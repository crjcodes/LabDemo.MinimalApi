# Purpose

What continuous integration and continuous deployment through GitHub Actions does this app support?

I also used this API as the basis for a series of posts at [CodeOnward.com](https://codeonward.com/category/net/lab-demo/).

# How to Use

Will be configured to automatically, on a git push, to run all the way through deployment to Azure, but
the actual deploy to Azure will be a manual click in GitHub.

If you want to re-run a job, just navigate to the repo's (you've created your own repo based on this?) GitHub Actions, select the workflow and action, and click on the retry icon.

# Future

- local, master, release branch support
- Manual deployment to more than one destination

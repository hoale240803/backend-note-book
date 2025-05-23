1. You can search stack of beanstalk you want
```
aws elasticbeanstalk list-available-solution-stacks --query "SolutionStacks[?contains(@,'Node.js')]" --output text
```



```
mkdir /c/Workspace/backend-note-book/devops/front-end/eb-deployment
cp -r /c/Workspace/backend-note-book/devops/front-end/build /c/Workspace/backend-note-book/devops/front-end/eb-deployment/
cp /c/Workspace/backend-note-book/devops/front-end/server.js /c/Workspace/backend-note-book/devops/front-end/eb-deployment/
cp /c/Workspace/backend-note-book/devops/front-end/package.json /c/Workspace/backend-note-book/devops/front-end/eb-deployment/
cp /c/Workspace/backend-note-book/devops/front-end/package-lock.json /c/Workspace/backend-note-book/devops/front-end/eb-deployment/
```

```
cd /c/Workspace/backend-note-book/devops/front-end/eb-deployment
zip -r ../app.zip .
```
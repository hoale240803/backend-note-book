# Infrastructure as code
# Gitops

# What is gitops?
- Lỗi về mặt con người khi thực hiện các hành động trên cloud service. Nên ta chỉ muốn chỉ có những agent như Github Action, Git lab, Circle CI, ... tác động vào cloud service bằng code. Để điểu khiển các service đó thì cần phải code bằng Terraform.

## Project architecture

## Prepare github repo

## Gihub secrets
### 1.Create ssh-key from you local machine
### 2.Export your ssh key
```
ssh-add c:/Users/lehoa/.ssh/<your_file_name>
```

if have counted any error as ssh **Could not open a connection to your authentication agent.**. please 
```
eval "$(ssh-agent -s)"
```

if still error check whether is there any git file **config** in **~/.ssh**. if not, create it
```
Host github.com
  HostName github.com
  User git
  IdentityFile ~/.ssh/<your_file_name>
```

## Terraform code
## Staging workflow wfor Terraform code
## Main Workflow for Terrform code
## Workflow for application code
## Docker build & Publish
## Deploy to EKS
## Clean up


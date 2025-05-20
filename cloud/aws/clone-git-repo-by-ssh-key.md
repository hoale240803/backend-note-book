# Setting Up SSH Keys for Git on Windows

This guide explains how to generate an SSH key, configure it for GitHub, GitLab, and Bitbucket, and use it to clone repositories on Windows using Git Bash. Ensure you have [Git for Windows](https://git-scm.com/download/win) installed, which provides Git Bash and SSH tools.

## 1. Generate an SSH Key
1. Open **Git Bash** (available after installing Git for Windows).
2. Run the following command to generate an SSH key pair:
```bash
ssh-keygen -t rsa -b 4096 -C "your_email@example.com"
```
- Press Enter to accept the default file location (~/.ssh/id_rsa).
- Enter a passphrase (optional) or press Enter to skip.
- This creates a private key (~/.ssh/id_rsa) and a public key (~/.ssh/id_rsa.pub).

- Ensure the private key has correct permissions
```
chmod 600 ~/.ssh/id_rsa
```
## 2. Add Your Key to the SSH Agent
Start the SSH agent in Git Bash:
```
eval "$(ssh-agent -s)"
```
This outputs a process ID (e.g., Agent pid 1234).

Add your private key to the agent:
```
ssh-add ~/.ssh/id_rsa
```

Verify the key was added:
```
ssh-add -l
```


## 3. Configure SSH for Multiple Services
Create or edit the SSH configuration file to manage keys for GitHub, GitLab, and Bitbucket.

Open (or create) the ~/.ssh/config file:
```
touch ~/.ssh/config
chmod 600 ~/.ssh/config
```

Edit the file with a text editor (e.g., nano ~/.ssh/config) and add
```
# Github
Host github.com
  HostName github.com
  User git
  IdentityFile ~/.ssh/<your_ssh_key>
  
# GitLab
Host gitlab.com
  HostName gitlab.com
  User git
  IdentityFile ~/.ssh/<your_ssh_key>

# Bitbucket
Host bitbucket.org
  HostName bitbucket.org
  User git
  IdentityFile ~/.ssh/<your_ssh_key>
```

## 4. Add your public key to Remote reporsitories
```
cat ~/.ssh/id_rsa.pub
```
Then add it to the respective service:

GitHub:
Go to Settings > SSH and GPG keys > New SSH key (or GitHub SSH Docs).
Paste the public key, give it a title, and click Add SSH key.

GitLab:
Go to Settings > SSH Keys (or GitLab SSH Docs).
Paste the public key, name it, and click Add key.

Bitbucket:
Go to Personal settings > SSH keys > Add key (or Bitbucket SSH Docs).
Paste the public key, label it, and click Add key.

## 5. Test Your SSH Connection
Verify your SSH setup for each service:

```
# GitHub
ssh -T git@github.com

# GitLab
ssh -T git@gitlab.com

# Bitbucket
ssh -T git@bitbucket.org
```
You should see a welcome message (e.g., Hi username! for GitHub or Welcome to GitLab, @username!).

## 6. Clone a Repository
Use the SSH URL to clone a repository. For example:
```
# GitHub
git clone git@github.com:username/repository.git

# GitLab
git clone git@gitlab.com:username/repository.git

# Bitbucket
git clone git@bitbucket.org:username/repository.git
```

For more details, refer to the official documentation:

[Github SSH Guide](https://docs.github.com/en/authentication/connecting-to-github-with-ssh/generating-a-new-ssh-key-and-adding-it-to-the-ssh-agent)

[Gitlab SSH Guide](https://docs.gitlab.com/user/ssh/)

[Bitbucket SSH Guide](https://support.atlassian.com/bitbucket-cloud/docs/set-up-personal-ssh-keys-on-windows/)
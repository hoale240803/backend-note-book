Deploying a ReactJS Frontend on AWS with Amplify
**amplify.yml**
```
version: 1
frontend:
  phases:
    preBuild:
      commands:
        - cd devops/front-end          # 👈 move into the app folder
        - npm ci                       # clean install
    build:
      commands:
        - npm run build
  artifacts:
    baseDirectory: devops/front-end/build   # ← where the built index.html ends up
    files:
      - '**/*'
  cache:
    paths:
      - devops/front-end/node_modules/**/*

```

| #                             | What it does                                                                              | Why it matters / gotchas                                                                              | Official doc             |
| ----------------------------- | ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------- | ------------------------ |
| **0** (`version: 1`)          | Declares build-spec schema version. Needed so Amplify parses keys correctly.              | Only `1` is valid today.                                                                              | ([AWS Documentation][1]) |
| **0.1** (`frontend:`)         | Top-level section for SPA / static sites.                                                 | Back-end resources (GraphQL, Auth, etc.) have their own section if used.                              | ([AWS Documentation][1]) |
| **1** (`cd devops/front-end`) | Move the build context into the folder that **contains package.json**.                    | If you skip this, Amplify runs `npm` in repo root and throws the `ENOENT package.json` error you saw. | ([AWS Documentation][2]) |
| **2** (`npm ci`)              | Installs exact versions from `package-lock.json`.                                         | Faster and reproducible; cache step (⑥) keeps the folder for next build.                              | ([AWS Documentation][1]) |
| **3** (`npm run build`)       | Executes the script that generates static assets (React → `build/`, Vue → `dist/`, etc.). | Must output an **`index.html`**; Amplify serves whatever lands in ④.                                  | ([AWS Documentation][3]) |
| **4** (`baseDirectory`)       | Path (relative to repo root) that now **contains your build artifacts** after step 3.     | If this is wrong you’ll get the “Welcome/first deployment” placeholder.                               | ([AWS Documentation][4]) |
| **5** (`files`)               | Glob patterns to upload. `**/*` grabs everything in ④.                                    | Narrow it (e.g., `*.html`, `static/**`) for smaller uploads if needed.                                | ([AWS Documentation][1]) |
| **6** (`cache.paths`)         | Directories persisted between builds to speed up `npm ci`.                                | Path must be relative to repo root (same as ① after `cd`).                                            | ([AWS Documentation][1]) |

[1]: https://docs.aws.amazon.com/amplify/latest/userguide/yml-specification-syntax.html?utm_source=chatgpt.com "Understanding the build specification - AWS Amplify Hosting"
[2]: https://docs.aws.amazon.com/amplify/latest/userguide/monorepo-configuration.html?utm_source=chatgpt.com "Configuring monorepo build settings - AWS Amplify Hosting"
[3]: https://docs.aws.amazon.com/en_us/amplify/latest/userguide/deploy-nextjs-app.html?utm_source=chatgpt.com "Deploying a Next.js SSR application to Amplify - AWS Amplify Hosting"
[4]: https://docs.aws.amazon.com/amplify/latest/userguide/build-settings.html?utm_source=chatgpt.com "Configuring the build settings for an app - AWS Amplify Hosting"

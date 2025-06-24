# Copilot Instructions: Unity Mobile Project Best Practices

Welcome to the project! This guide outlines best practices for working in a Unity mobile simulation game project. Please follow these guidelines to ensure code quality, maintainability, and smooth collaboration.

## 1. Project Structure
- Organize scripts into meaningful folders (e.g., `Assets/Scripts/Gameplay`, `Assets/Scripts/UI`, `Assets/Scripts/Managers`).
- Keep assets (sprites, audio, prefabs) in dedicated folders under `Assets/`.
- Use namespaces to avoid naming conflicts.

## 2. Mobile Optimization
- Use Unity's Profiler to monitor performance on target devices.
- Optimize textures and audio for mobile (compression, resolution, formats).
- Minimize use of real-time lights and expensive shaders.
- Pool objects instead of instantiating/destroying frequently.
- Test on actual devices regularly.

## 3. Version Control
- Commit early and often with clear messages.
- Exclude `Library/`, `Temp/`, and other generated folders from version control.
- Use Unity's YAML merge for scenes and prefabs when possible.

## 4. Testing-First Development
- **Write tests before implementing features.**
- Use Unity Test Framework (EditMode and PlayMode tests) for all new gameplay systems and components.
- Place EditMode tests in `Assets/Tests/Editor/` and PlayMode tests in `Assets/Tests/PlayMode/` or similar dedicated folders.
- Ensure all tests pass before merging code.
- Refactor code to improve testability (e.g., use dependency injection, avoid static state).

## 5. Code Quality
- Follow C# coding conventions (PascalCase for classes/methods, camelCase for variables).
- Use descriptive names for variables, methods, and classes.
- Add XML documentation to public APIs.
- Keep methods short and focused.
- Use `SerializeField` for private fields that need to be set in the Inspector.

## 6. Prefabs and Scenes
- Use prefabs for reusable objects.
- Avoid making direct changes to prefab instances in scenes; apply changes to the prefab asset.
- Keep scenes modular and focused (e.g., separate gameplay, UI, and test scenes).

## 7. Collaboration
- Communicate changes in shared assets (scenes, prefabs) to the team.
- Pull latest changes before starting new work.
- Resolve merge conflicts promptly and carefully.

## 8. Documentation
- Document new systems and complex logic in code and in the project wiki/readme.
- Update this guide as new best practices emerge.

---

By following these practices, we ensure a robust, maintainable, and high-quality Unity mobile simulation game. Happy coding!

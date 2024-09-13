# Performance aware space shooter

This repo contains a simple collection of DOTS components and systems, made for  a school assignment at futuregames.
The project is fully implemented in DOTS, using Jobs and the BurstCompiler when appropriate to guarantee extra performance in the implemented systems.
When appropriate (like in the SpawnerSystems) the burstcompiled jobs have been scheduled in parallel.
Despite this being a toy project for learning purposes and containing no real gameplay, attention was put in code organization and style, reusing the
same components when possible and providing an authoring script with the related bake for each component, allowing a in-editor experience akin to what is the standard GameObject workflow.

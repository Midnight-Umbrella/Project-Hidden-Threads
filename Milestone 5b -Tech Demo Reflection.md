During the tech demo playthrough, we received a combination of positive feedback and constructive criticism that helped clarify both our strengths and the areas that need improvement before the final milestone.

Overall, the core systems of the game functioned reliably. The phone interface worked as intended, including switching between apps and returning to the home screen. Scene transitions between the title screen, cutscene, and main gameplay scene were stable. The inventory and clue systems were functional and demonstrated the intended gameplay concept of switching between the phone and the real-world environment. The art style was also positively received and described as visually cohesive and appealing, which reassured us that our visual direction is strong.

However, several technical and design shortcomings became apparent during the demo.

One of the major issues was the lack of animated sprites for the player character. Without walking animations, the character's movement felt stiff and unnatural. Combined with the current camera behavior, movement sometimes appeared as if the character was snapping or teleporting toward the center of the screen rather than being smoothly followed by the camera. While some of this may be perceptual and will likely improve once animations are implemented, it also highlights the need to potentially introduce camera smoothing or interpolation to create a more polished feel.

Another issue was the absence of continuous looping background music. The lack of persistent audio made the game world feel less immersive and somewhat empty. This was a clear production oversight, and implementing a properly managed looping BGM system will be a priority moving forward.

We also lost marks because our dialogue system was not yet implemented using a CSV-driven structure. While the dialogue functions correctly, it is currently hard-coded, which limits scalability and does not align with the technical expectations of the milestone. Transitioning to a CSV-based dialogue system will significantly improve maintainability and future content expansion.

During the opening cutscene, the dialogue text advanced too quickly, making it difficult to read. This revealed a usability issue rather than a purely technical one. We need to implement adjustable text speed, or allow user-controlled progression so that players can comfortably follow the narrative. The cutscene should introduce the story clearly, not overwhelm the player.

Another important piece of feedback concerned interaction consistency. Different systems used different keys to close interactions (Space, Esc, P), which created confusion. Standardizing how interactions are closed will improve user experience. We plan to unify interaction behavior, likely using Esc as a universal close command and allowing mouse clicks or Space to advance dialogue. Additionally, item-triggered dialogue boxes should automatically disappear when the player moves away, making the system feel more natural and intuitive.

The demo helped us recognize that while our foundational systems are structurally sound, we are now entering a phase where polish, consistency, and user experience refinement are more important than adding new features. The core mechanics are functional, but animation, audio, smoothing, and interface standardization will significantly elevate the perceived quality of the game.

In summary, the tech demo validated that our architecture and gameplay loop are viable. However, it also highlighted the gap between a functional prototype and a production-ready experience. Our next sprint will prioritize animation integration, camera smoothing, BGM looping, CSV-based dialogue implementation, and standardized interaction controls to elevate the overall game quality.

It is also important to note that the reflections above are written from my perspective as the Producer managing this team through the demo milestone. Beyond the technical and design issues, I believe our time management still has significant weaknesses. Several features were implemented close to the deadline, which limited our ability to properly polish, test, and refine them.

 

For the next phase of development, I intend to strengthen our risk management planning. This includes identifying high-risk systems earlier, allocating buffer time for integration and polish, and setting clearer internal deadlines before official milestone submissions. Reducing last-minute implementation will allow us to focus not just on functionality, but on quality.

Ultimately, good games require time to iterate and refine. Moving forward, my priority as Producer is to create a development structure that allows the team the space and stability needed to polish the experience properly, rather than rushing to meet minimum functional requirements.   

---
 Alan Yu                                                                                                                                                      
 On behalf of Group Midnight Umbrella                                                                                                                                           

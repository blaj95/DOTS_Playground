using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ECS : MonoBehaviour
{
// ENTITY
    // The THINGS that populate our game or program.
    // INDICIES - also can think of them as IDs.
    // They themselves DO NOT CONTAIN any data or do anything.

// COMPONENT
    // THE DATA
    // Organized by the data itself and not the entity (One of the key differences between OOP).
    
// SYSTEM
    // CODE that TRANSFORMS the DATA
    // Systems query only the components they need and transform that data.
    // Fast because of the layout of the memory.
    
//ECS CONCEPTS
    // Think about it as IDENTITY(Entities), DATA(Components) and BEHAVIOUR(Systems).
    // SYSTEMS do not care about COMPONENTS unless specified to.
    // ARCHETYPES are a unique combination of components.
        // Archetypes of an entity determine where the components of that entity are stored.
    // ECS Allocates memory in chunks.
        // ArchetypeChunks are the objects that represent this allocated memory.
        // A chunk always contains entities of a single archetype.
        // When a chunk becomes full, a new chunk is allocated for any new entities of the same archetype.
        // If you add/remove components of any archetype, the components of an archetype are moved to a different chunk.
    // Finding entities with given components only requires a search through existing archetypes rather than all entities.
    // Values of shared components in an archetype also determine which entities are stored in chunks.
        // All entities in a given chunk have the same exact values for any shared components. 
        // When a component value is changed, the modified entity moves to a different chunk.
        // Use shared components to group entities within an archetype when it is more efficient to process them together.
//SYSTEM ORGANIZATION
    //ECS organizes systems by WORLD and then by GROUP
    // You can specify the update order of systems within the same group
    
//ENTITY QUERIES
    // Use an EntityQuery to identify which entities a system should process.
    // EntityQuery searches existing archetypes for those having components that match your requirements.
    // 3 Different Requirements for a Query
        // All - The archetype must contain all of the component types in the All category
        // Any - The archetype must contain all of the component types in the All category
        // None - The archetype must not contain any of the component types in the None category.
    // EntityQuery provides a list of chunks containing the types of components required by the query.
    // You can iterate over the components in these chunks using:
        // IJobChunk
        // IJobForEach - Implicitly creates an entity query based on the Job parameters and attributes. You can override the implicit query when scheduling the Job.
        // Or a non-Job for-each Loop
    
//JOBS
    // Job system transforms data outside of the main thread.
    // ECS Jobs use an EntityQuery that defines which components a job accesses but also specifies if the access is Read-Only or Read-Write.
        // Defines which jobs run in parallel and run in sequence
        // Jobs that read the same data can run at the same time, but when one Job writes to data that another Job accesses, those Jobs must run in sequence.
        
    


}

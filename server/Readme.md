# Quick Tips

## Before you click
- Not every permutation of tenants/cosmos/elastic has been tested, if you find a flaw, don't be afraid to adjust the xsl.
- Entity Framework is database first, see generation script in Placeholder.Data.Sql/readme.md for instructions on how to implement.

## Get it Running
- Find all instances of `your-` and replace as needed.
- - Requires SQL, and Cosmos for initial load. (blob if you want to use assets)
- This supports multi tenancy, it expects mapping from an entity to a tenant.
- - Tenants are via string, not guids.
- - You can manage the mapping, or hard code if not needed.
- - Tenants require database connection strings. `primary` is the default/implied tenant.
- Once compiled and debugging, navigate to /bootstrap.
- - That will create your account.
- - Use that account to adminster the system.


## Stuff to finish
- After you see it run, Recommend replace `Placeholder` and `placeholder` with your own product name. (note, VS can do all content, but don't forget to rename files)
- Security Enforcement is demonstrative only
- Email sending is commented out
- Change crypo keys and jwt keys and any key like it.

## Notes
- RestSharp is not the most recent on purpose (annoying breaking change that makes it hard to share sdk with mobile platform)
- Startup.cs has functional integrations with Zero.Foundation, but it may not be flawless. 
- Reminder: Lean towards Unity (via Foundation), not the built in container.

## Double Reminder
- Stencil Server is a paradigm, convention, and starter pack.
- Learn and customize - don't wait for patches.
- Zero.Foundation is patchable, however, so contribute/connect if issues arise.
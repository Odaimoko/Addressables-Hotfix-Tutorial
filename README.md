# Addressables Hotfix Tutorial

This is the documentation for my educational repository [AddressablesTutorial](https://github.com/Odaimoko/AddressablesTutorial), as a record of my learning experience.

This serves as an intermediate level resource. You will need at least to know how to load resources using Addressables inside a local build.

I will only show you the minimalistic examples for you to build upon, perfectly in the network of your company or your home. I'm not gonna talk about how to host your build output on some server.

This tutorial is written with Addressables `1.19.19` and Unity `2021.3.29f1`, developed on MacOS, deployed on MacOS and Android. I believe it works from Unity `2020.3` and Addressables `1.16+`, for any platform (Windows). Although there could be some buttons or checkboxes moved around since then, in which case please let me know via [GitHub Issues](https://github.com/Odaimoko/AddressablesTutorial/issues).

# Tutorial Content
[Part 1: Introduction](https://imoko.cc/blog/imk/AddressablesTutorial/Addressables-Introduction)

[Part 2: Best Practice](
https://imoko.cc/blog/imk/AddressablesTutorial/Addressables-Best-Practice)

# The Scene
![img.png](./readme_assets/Pasted%20image%2020240306204900.png)
- (1) Basic Addressables paths. The screenshot shows the paths in Unity Editor on MacOS system.
- (2) and (3) are Texts to indicate some Asset Bundles have been changed. The button `LoadLatest` fetches the data from the latest bundle and release the handle immediately after.
- (4) and (5) loads images and display them. (4) only shows images from `LocalStatic` group, while (5) shows images dynamically loaded from several `Remote*` groups.
- (6) checks if the catalog needs update. If so, (7) will show up which downloads the catalog and the associated bundles.
- (8) clears all the cached bundles, but preserve the latest catalog. It frees up user spaces. A user needs to redownload the bundle after clearance.
- (9) logs some other info to Unity Console.
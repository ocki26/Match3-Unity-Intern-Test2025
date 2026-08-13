# Match3-Unity-Intern-Test2025

**Assessment Test: Intern Unity Developer**
- **Company:** Winter Wolf - IEC Games
- **Unity Version:** 2020.3.38f+
- **Programming Language:** C#

---

## 📌 Progress & Tasks Overview

### ✅ Task 1: Re-skin
- **Description:** Re-skinned all default items/blocks into Fish using assets provided in `Assets/Textures/Fish/`.
- **Implementation:**
  - Items are instantiated dynamically via `Resources.Load<GameObject>()` from `Assets/Resources/prefabs/`.
  - Re-skinning was achieved by updating the `Sprite` property in `SpriteRenderer` across `itemNormal01.prefab` to `itemNormal07.prefab`.
  - Asset Mapping:
    - `itemNormal01.prefab` ➔ `fish_1.png`
    - `itemNormal02.prefab` ➔ `fish_2.png`
    - `itemNormal03.prefab` ➔ `fish_3.png`
    - `itemNormal04.prefab` ➔ `fish_4.png`
    - `itemNormal05.prefab` ➔ `fish_5.png`
    - `itemNormal06.prefab` ➔ `fish_6.png`
    - `itemNormal07.prefab` ➔ `rainbow_fish.png`

---

### ✅ Task 2: Change the Gameplay (Tile-Match / Triple Match)
- **Description:** Transformed the match-3 gameplay into a Tile-Match (Triple Match) mechanism with a bottom tray area.
- **Implementation:**
  1. **Board Generation ([Board.cs](file:///d:/Code/Match3-Unity-Intern-Test2025/Assets/Scripts/Board/Board.cs)):** All fish items are generated in complete triplets (divisible by 3) and shuffled randomly across the board.
  2. **Bottom Tray ([TrayController.cs](file:///d:/Code/Match3-Unity-Intern-Test2025/Assets/Scripts/Controllers/TrayController.cs)):** Bottom area holds a maximum of 5 cells. Accumulating 3 identical fish types clears them and shifts remaining items to the left.
  3. **Interaction & Gameplay ([BoardController.cs](file:///d:/Code/Match3-Unity-Intern-Test2025/Assets/Scripts/Controllers/BoardController.cs)):** Tapping a board item moves it into the tray.
  4. **Win / Lose Evaluation:**
     - **Win:** All board cells are cleared and tray is empty.
     - **Lose:** Tray is full (5 items) with no match-3 possible.
  5. **AI Autoplay & Auto Lose ([AutoplayController.cs](file:///d:/Code/Match3-Unity-Intern-Test2025/Assets/Scripts/Controllers/AutoplayController.cs)):**
     - **Autoplay (Auto Win):** AI prioritizes completing triplets with a 0.5s delay per action.
     - **Auto Lose:** AI selectively gathers 5 distinct fish types to fill the tray with a 0.5s delay.
  6. **UI Integration:** [UIPanelMain.cs](file:///d:/Code/Match3-Unity-Intern-Test2025/Assets/Scripts/UI/UIPanelMain.cs), [UIMainManager.cs](file:///d:/Code/Match3-Unity-Intern-Test2025/Assets/Scripts/UI/UIMainManager.cs), and [UIPanelGameOver.cs](file:///d:/Code/Match3-Unity-Intern-Test2025/Assets/Scripts/UI/UIPanelGameOver.cs).

---

### ✅ Task 3: Improve Gameplay & Time Attack Mode
- **Description:** Enhanced animations, ensured fish type variety, and introduced Time Attack Mode.
- **Implementation:**
  1. **Fish Variety Guarantee ([Board.cs](file:///d:/Code/Match3-Unity-Intern-Test2025/Assets/Scripts/Board/Board.cs)):** The initial board is guaranteed to include all 7 fish types (`TYPE_ONE` to `TYPE_SEVEN`) among the generated triplets.
  2. **Enhanced Animations ([Item.cs](file:///d:/Code/Match3-Unity-Intern-Test2025/Assets/Scripts/Board/Item.cs) & [TrayController.cs](file:///d:/Code/Match3-Unity-Intern-Test2025/Assets/Scripts/Controllers/TrayController.cs)):**
     - Smooth scale-punch and ease movements when an item travels from the board to the tray.
     - Scale-to-zero animation (`DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack)`) when 3 identical items are cleared.
  3. **Time Attack Mode ([GameManager.cs](file:///d:/Code/Match3-Unity-Intern-Test2025/Assets/Scripts/Controllers/GameManager.cs), [BoardController.cs](file:///d:/Code/Match3-Unity-Intern-Test2025/Assets/Scripts/Controllers/BoardController.cs), [TrayController.cs](file:///d:/Code/Match3-Unity-Intern-Test2025/Assets/Scripts/Controllers/TrayController.cs)):**
     - Accessible via the **Time Attack** button on the Home screen.
     - 1-minute countdown timer (60s) displayed on screen.
     - Players do **not** lose when the 5-slot tray is full.
     - Players can **tap an item in the tray to return it to its original board cell**, freeing up tray capacity.
     - The player loses only if the 1-minute timer expires before clearing the board.

# Match3-Unity-Intern-Test2025

**Assessment Test: Intern Unity Developer**
- **Company:** Winter Wolf - IEC Games
- **Unity Version:** 2020.3.38f+
- **Programming Language:** C#

---

## 📌 Progress & Tasks Overview

### ✅ Task 1: Re-skin
- **Mô tả:** Thay đổi toàn bộ skin của các item/blocks từ hình dạng cũ sang các loại Cá (Fish) sử dụng tài nguyên có sẵn trong thư mục `Assets/Textures/Fish/`.
- **Cách thức thực hiện (Implementation):**
  - Hệ thống khởi tạo item động thông qua hàm `Resources.Load<GameObject>()` dựa trên các Prefab được lưu trữ tại đường dẫn `Assets/Resources/prefabs/`.
  - Để thay đổi icon/giao diện cho các blocks (items), **chỉ cần thay đổi trường Sprite trong component `SpriteRenderer` của các file Prefab tương ứng** (`itemNormal01.prefab` đến `itemNormal07.prefab`), không cần can thiệp hay chỉnh sửa code logic.
  - Danh sách mapping:
    - `itemNormal01.prefab` ➔ `fish_1.png`
    - `itemNormal02.prefab` ➔ `fish_2.png`
    - `itemNormal03.prefab` ➔ `fish_3.png`
    - `itemNormal04.prefab` ➔ `fish_4.png`
    - `itemNormal05.prefab` ➔ `fish_5.png`
    - `itemNormal06.prefab` ➔ `fish_6.png`
    - `itemNormal07.prefab` ➔ `rainbow_fish.png`

---

### ⏳ Task 2: Change the Gameplay (Upcoming)
- Chuyển đổi gameplay từ Match-3 truyền thống sang cơ chế **Tile-match (Triple Match)**:
  1. Nhấn vào item trên bàn cờ để chuyển xuống khay chứa 5 ô ở đáy (Bottom cells).
  2. Gom đủ 3 item giống nhau ở khay dưới sẽ tự động xóa (Clear).
  3. Thắng khi dọn sạch bàn cờ; Thua khi khay 5 ô bị lấp đầy.
  4. Số lượng mỗi loại item khởi tạo trên bàn cờ luôn chia hết cho 3.
  5. Thêm màn hình Home với tính năng **Autoplay** (tự động giải để thắng) và **Auto Lose** (tự động chơi để thua) với độ trễ 0.5s mỗi hành động.

---

### ⏳ Task 3: Improve Gameplay & Time Attack Mode (Upcoming)
- Đảm bảo bàn cờ ban đầu luôn chứa đủ tất cả các loại cá.
- Bổ sung animation di chuyển item xuống khay và animation nổ/scale khi match 3 item.
- Bổ sung chế độ chơi mới: **Time Attack Mode** (Thời gian 1 phút, cho phép bấm trả item từ khay về bàn cờ, không thua khi khay đầy).

# 🚀 Quick Start Guide - Avina

## تیاری

### ✅ تمام Features موجود ہیں

1. **Sidebar Navigation** ✅
2. **Dark Theme** ✅
3. **Home Page** ✅
4. **School Page** ✅
5. **Games Page** ✅
6. **Store Page** ✅
7. **Profile Page** ✅
8. **Upload Content Page** ✅
9. **Animations** ✅

---

## 🎬 شروع کرنے کے لیے

### Step 1: تصاویر شامل کریں

فولڈرز میں اپنی تصاویر ڈالیں:

```
wwwroot/images/
├── courses/          (800x600px)
│   ├── math.jpg
│   ├── science.jpg
│   ├── skills.jpg
│   ├── farsi.jpg
│   ├── creative.jpg
│   └── programming.jpg
├── games/            (800x600px)
│   ├── math-challenge.jpg
│   ├── word-smart.jpg
│   ├── logic-puzzle.jpg
│   ├── world-geo.jpg
│   ├── sequences.jpg
│   └── rainbow.jpg
├── products/         (400x400px)
│   ├── creativity-kit.jpg
│   ├── board-game.jpg
│   ├── story-book.jpg
│   ├── video-course.jpg
│   ├── audiobook.jpg
│   └── premium-access.jpg
├── profiles/         (300x300px)
│   └── user1.jpg
└── hero/             (1200x600px)
    └── hero-image.jpg
```

### Step 2: پروجیکٹ چلائیں

```powershell
cd C:\Users\Mehrd\source\repos\Avina
dotnet run
```

### Step 3: مرورگر میں کھولیں

```
http://localhost:5000
```

---

## 📱 صفحات

| صفحہ | URL | مقصد |
|------|-----|------|
| 🏠 Home | / | صفحہ اول |
| 📚 School | /school | دوره‌های آموزشی |
| 🎮 Games | /games | بازی‌های آموزشی |
| 🛍️ Store | /store | فروشگاه |
| 👤 Profile | /profile | پروفایل کاربر |
| 📤 Upload | /upload | اپلود محتوا |

---

## 🎨 Features

### 🎬 Animations
- Smooth entrance animations
- Staggered card loading
- Hover effects
- Transitions

### 🌙 Dark Theme
- Purple & Pink gradients
- Professional colors
- High contrast
- Eye-friendly

### 📐 Responsive
- Desktop
- Tablet
- Mobile

### 🔍 Functionality
- Search
- Filter
- Navigation
- Forms

---

## 💡 نکات

### Sidebar
- RTL (دائیں سے بائیں)
- User profile
- Navigation items
- Animations

### Header
- Search box
- Notifications
- Settings

### Content
- Grid layout
- Animated cards
- Smooth loading
- Interactive

---

## 🔧 Customization

### رنگ تبدیل کریں

`app-custom.css` میں:
```css
--primary-color: #7c3aed;
--secondary-color: #ec4899;
```

### متن تبدیل کریں

Razor files میں مطابق تبدیلی کریں

### فارم شامل کریں

Upload.razor میں نئے fields شامل کریں

---

## 📚 فائلیں

### Pages
- `Home.razor` - صفحہ اول
- `School.razor` - مدرسہ
- `Games.razor` - بازی‌ها
- `Store.razor` - فروشگاه
- `Profile.razor` - پروفایل
- `Upload.razor` - اپلود

### CSS
- `app.css` - بنیادی
- `app-custom.css` - Animations
- `school.css` - School page
- `games.css` - Games page
- `store.css` - Store page
- `profile.css` - Profile page

### Services
- `UserService.cs` - کاربران
- `CourseService.cs` - دوره‌ها

---

## ✅ Checklist

- [ ] تصاویر شامل کیے؟
- [ ] پروجیکٹ چلایا؟
- [ ] Sidebar دیکھا؟
- [ ] Dark theme دیکھا؟
- [ ] Animations دیکھا؟
- [ ] Upload page دیکھا؟

---

## 🆘 مسائل

### تصاویر نہیں دیکھ رہے؟
- فولڈر path check کریں
- فائل نام match کریں
- Browser cache clear کریں

### Styles apply نہیں ہو رہے؟
- CSS لنک check کریں
- F12 کھولیں (Dev tools)
- Network tab میں CSS دیکھیں

### Animations کام نہیں کر رہے؟
- Browser کو reload کریں
- Cache clear کریں
- دوسرے browser میں try کریں

---

## 📞 Support

تمام فائلیں تیار ہیں۔ صرف:
1. تصاویر شامل کریں
2. پروجیکٹ چلائیں
3. لطف لیں! 🎉

---

**خوش آمدید! آپ کا پروژہ تیار ہے! 🚀**

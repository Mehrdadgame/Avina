# 📸 راهنمای قرار‌دادن تصاویر

این فایل توضیح می‌دهد که تصاویر را کجا قرار دهید تا پروژه درست کار کند.

## 📁 ساختار فولدرهای تصاویر

تمام تصاویر باید در فولدر `wwwroot/images` قرار داشته باشند:

```
Avina/wwwroot/images/
├── hero/              # تصاویر صفحه اول
│   └── hero-image.jpg
├── courses/           # تصاویر دوره‌های آموزشی
│   ├── math.jpg
│   ├── science.jpg
│   ├── skills.jpg
│   ├── farsi.jpg
│   ├── creative.jpg
│   └── programming.jpg
├── games/             # تصاویر بازی‌های آموزشی
│   ├── math-challenge.jpg
│   ├── word-smart.jpg
│   ├── logic-puzzle.jpg
│   ├── world-geo.jpg
│   ├── sequences.jpg
│   └── rainbow.jpg
├── products/          # تصاویر محصولات فیزیکی
│   ├── creativity-kit.jpg
│   ├── board-game.jpg
│   ├── story-book.jpg
│   ├── video-course.jpg
│   ├── audiobook.jpg
│   └── premium-access.jpg
└── profiles/          # تصاویر پروفایل کاربران
    └── user1.jpg
```

## 🖼️ مشخصات تصاویر

### تصاویر دوره‌ها (Courses)
- **اندازه:** 800×600 px (حداقل)
- **فرمت:** JPG/PNG
- **وزن:** کمتر از 500 KB
- **نسبت ابعاد:** 4:3

### تصاویر بازی‌ها (Games)
- **اندازه:** 800×600 px (حداقل)
- **فرمت:** JPG/PNG
- **وزن:** کمتر از 500 KB
- **نسبت ابعاد:** 4:3

### تصاویر محصولات (Products)
- **اندازه:** 400×400 px (حداقل)
- **فرمت:** JPG/PNG
- **وزن:** کمتر از 300 KB
- **نسبت ابعاد:** 1:1

### تصاویر پروفایل (Profiles)
- **اندازه:** 300×300 px (حداقل)
- **فرمت:** JPG/PNG
- **وزن:** کمتر از 200 KB
- **نسبت ابعاد:** 1:1 (دایره‌ای)

### تصاویر قهرمان (Hero)
- **اندازه:** 1200×600 px (حداقل)
- **فرمت:** JPG/PNG
- **وزن:** کمتر از 1 MB
- **نسبت ابعاد:** 2:1

## 🚀 نحوه اضافه کردن تصاویر

### روش 1: استفاده از File Explorer

1. پوشه `Avina/wwwroot/images` را باز کنید
2. تصویر را در پوشه مربوطه قرار دهید
3. نام فایل را طبق لیست بالا تنظیم کنید
4. برنامه را بازآغاز کنید

### روش 2: استفاده از Terminal

```bash
# برای Windows
xcopy "C:\Users\[YourUsername]\Pictures\image.jpg" "C:\Users\Mehrd\source\repos\Avina\Avina\wwwroot\images\courses\"

# برای Linux/Mac
cp ~/Pictures/image.jpg ~/Avina/wwwroot/images/courses/
```

## 📥 دانلود نمونه تصاویر

برای کار کردن سریع، می‌توانید از وب‌سایت‌های زیر تصاویر دانلود کنید:

- **Unsplash:** https://unsplash.com
- **Pexels:** https://pexels.com
- **Pixabay:** https://pixabay.com
- **Freepik:** https://freepik.com

### کلمات کلیدی جستجو:

- دوره آموزشی: "online learning", "education", "study"
- بازی: "gaming", "puzzle game", "brain game"
- محصولات: "book", "kit", "educational tools"
- پروفایل: "person", "avatar", "portrait"

## ✅ بررسی تصاویر

پس از قرار دادن تصاویر:

1. **بازگشایی برنامه:**
```bash
dotnet run
```

2. **بررسی صفحات:**
   - [Home](http://localhost:5000/) - تصاویر دوره‌ها را بررسی کنید
   - [School](http://localhost:5000/school) - تصاویر دوره‌های آموزشی
   - [Games](http://localhost:5000/games) - تصاویر بازی‌ها
   - [Store](http://localhost:5000/store) - تصاویر محصولات
   - [Profile](http://localhost:5000/profile) - تصویر پروفایل

## 🎨 نکات طراحی

- تمام تصاویر باید حالتی حرفه‌ای داشته باشند
- رنگ‌ها باید با رنگ‌های اصلی برنامه هماهنگ باشند
- تصاویر باید بی‌کسل و شفاف باشند
- برای تصاویر شخصی، از افراد متنوع استفاده کنید

## 🔧 تغییر آدرس تصاویر

اگر می‌خواهید آدرس تصاویر را تغییر دهید:

**در فایل Razor Pages:**
```razor
<!-- Old -->
<img src="images/courses/math.jpg" />

<!-- New -->
<img src="/assets/images/courses/math.jpg" />
```

**در C# Models:**
```csharp
// Prefix را تغییر دهید
new Course 
{ 
    ThumbnailImage = "images/courses/math.jpg"  // تغییر دهید
}
```

## 📞 مشاهده خرابی‌ها

اگر تصاویر نمایش داده نمی‌شوند:

1. **کنسول مرورگر را بررسی کنید:** `F12 → Console`
2. **مسیر فایل را بررسی کنید:** آیا فایل در فولدر درست است؟
3. **نام فایل را بررسی کنید:** آیا نام کاملاً درست است؟
4. **اندازه فایل را بررسی کنید:** آیا بیش از حد بزرگ است؟

---

**نوت:** تمام تصاویر باید در فولدر `wwwroot` باشند تا قابل دسترسی از مرورگر باشند.

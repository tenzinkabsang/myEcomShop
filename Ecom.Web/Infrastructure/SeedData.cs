using Ecom.Core.Domain;
using Ecom.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Web.Infrastructure
{
    public static class SeedData
    {
        private static Random _rand = new();

        public static void Populate(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    Enumerable.Range(0, 3).SelectMany(x => new List<Product>
                {
                    new Product
                    {
                        Name = "Game Day Blue Football Sweatshirt",
                        Sku = $"PE{_rand.Next(x, 500)}",
                        ShortDescription = "A beautiful sweatshirt",
                        FullDescription = """
                        Hey there, football fan! Ready to elevate your game day style with a touch of rugged charm? Our Game Day Blue Football Sweatshirt features distressed text and a football graphic, perfect for showcasing your love for the game with a trendy edge. Whether you're at the stadium, hosting a tailgate party, or cheering from home, this sweatshirt combines comfort with a bold design. Crafted from ultra-soft, high-quality fabric, our Game Day Blue Football Sweatshirt ensures warmth and durability throughout the season. Its relaxed fit and premium material keep you cozy and stylish as you cheer for your team from kickoff to the final whistle. The distressed text and football graphic add a unique and rugged element to your game day wardrobe. This sweatshirt pairs effortlessly with jeans, leggings, or joggers, making it a standout piece for any football enthusiast. Unlike ordinary sweatshirts, ours stand out with their distinctive design and superior quality, promising lasting softness and color retention wash after wash. Whether you're celebrating victories or gearing up for the next big game, the Game Day Blue Football Sweatshirt is your go-to choice for stylishly embracing your football passion. Gear up for game day and showcase your football enthusiasm with our Game Day Blue Football Sweatshirt. Whether you're in the stands or relaxing at home, this sweatshirt ensures you're dressed comfortably and ready to support your team with a rugged, stylish look.
                        """,
                        Category = "Tops",
                        Price = 27.99m,
                        Images = [new Image {
                            IsMainImage = true,
                            ImageUrl = "https://images.jane.com/sellers/limeberry-designs/product-images/game-day-blue-fb-sw.jpg?preset=xlarge"
                        }]
                    },
                     new Product
                     {
                         Name = "Pocket Knitted Solid Color Cardigan",
                         Sku = $"PE{_rand.Next(x, 500)}",
                         ShortDescription = "Pocket Knitted Solid Color Cardigan features a knitted pattern with subtle fuzzy texturing, and an arc-shaped hem design, and is made of a soft and comfortable fabric.",
                         FullDescription = "Pocket Knitted Solid Color Cardigan features a knitted pattern with subtle fuzzy texturing, and an arc-shaped hem design, and is made of a soft and comfortable fabric.",
                         Category = "Tops",
                         Price = 48.95m,
                         Images = [new Image {
                             IsMainImage = true,
                             ImageUrl = "https://images.jane.com/sellers/gilbert-girls-boutique/product-images/2094762_square.jpg?preset=xlarge"
                         }]
                     },
                     new Product
                     {
                         Name = "Happy Turkey Day Autumn Thanksgiving",
                         Sku = $"PE{_rand.Next(x, 500)}",
                         ShortDescription = "Our boyfriend-fit t-shirts are crafted for those who value both comfort and style. With longer sleeves and a relaxed torso length, these tees are perfect for everyday wear. Each tee is hand-printed and made to order, ensuring high quality and attention to detail.",
                         FullDescription = """
                         Sizing Guide:
                         - Fit: Relaxed, unisex fit with longer sleeves and torso length.
                         - Men: Size up if between sizes.
                         - Women: Choose your usual size for a relaxed fit, or size down for a more fitted style.
                         - Customer Feedback: Most customers say these tees fit true to size.
                         - Quick Measurement: Small size length is 28 inches.
                         """,
                         Category = "Tops",
                         Price = 19.50m,
                         Images = [new Image{
                             IsMainImage = true,
                             ImageUrl = "https://images.jane.com/sellers/printed-peach/product-images/happy-turkey-day-trendy-fall-thanksgiving-family-festive-autumn-orange-bella-canvas-tee.jpg?preset=xlarge"
                         }]
                     },
                     new Product
                     {
                         Name = "It Get Worse Hysterical Graphic Tee",
                         Sku = $"PE{_rand.Next(x, 500)}",
                         ShortDescription = "Each graphic tee is printed with high quality eco-friendly inks on premium cotton Bella +Canvas tees for an ultra soft and comfortable fit.",
                         FullDescription = """
                         Don't just look good, feel good. Tees are unisex regular fit. For an oversized look, please consider sizing up. Please contact us if you'd like a specific color or different brand of shirt. All designs can be printed on adult & youth tees. 100% Cotton. Heathers: 52% Cotton, 48% Polyester Premium Quality: Crafted with love and attention to detail, our graphic tees are made using ultra soft and comfortable cotton based tees, ensuring a cozy fit for all-day wear. Our professional printing process guarantees vibrant colors that won't fade, allowing you to enjoy this design for many years to come. Handmade with Love: Each tee is individually crafted in our own printing studio to ensure a professional and high quality print.   
                         """,
                         Category = "Tops",
                         Price = 34.95m,
                         Images = [new Image {
                             IsMainImage = true,
                             ImageUrl = "https://images.jane.com/sellers/johnny-threads/product-images/itgetsworsemens.jpg?preset=xlarge"
                         }]
                     },
                     new Product
                     {
                         Name = "Stay warm and spread cheer with the Tis The Season To Be Jolly Sweatshirt!",
                         Sku = $"PE{_rand.Next(x, 500)}",
                         ShortDescription = "Flat-packed 35,000-seat stadium",
                         FullDescription = "Stay warm and spread cheer with the Tis The Season To Be Jolly Sweatshirt!",
                         Category = "Tops",
                         Price = 19m,
                         Images = [new Image {
                             IsMainImage = true,
                             ImageUrl = "https://images.jane.com/sellers/limeberry-designs/product-images/tis-season-ti-be-jolly-sw.jpg?preset=xlarge"
                         }]
                     },
                     new Product
                     {
                         Name = "Gold Hoop Earrings",
                         Sku = $"PE{_rand.Next(x, 500)}",
                         ShortDescription = "Elevate your boho-chic style with the Reagan Gold Hoop Earrings.",
                         FullDescription = """
                         These minimalist yet eye-catching earrings feature thin gold hoops adorned with delicate cream beaded details, adding a touch of texture and charm. The combination of the sleek gold hoops and the subtle beaded accents creates a captivating contrast, making these earrings a perfect accessory for both casual and dressed-up looks. With their versatile design and effortless elegance, the Reagan Gold Hoop Earrings effortlessly enhance any ensemble, adding a touch of bohemian flair to your everyday style. Whether you're attending a music festival or simply want to infuse your outfit with a hint of artistic allure, these earrings are a must-have for those seeking a stylish and minimalist accessory. Minimalist gold hoop earrings, Dainty cream bead details, Post backs, Approx. 2.25"
                         """,
                         Category = "Earrings",
                         Price = 16,
                         Images = [new Image {
                             IsMainImage = true,
                             ImageUrl = "https://images.jane.com/sellers/shop-suey-boutique/product-images/8051766722839.jpg?preset=xlarge"
                         }]
                     },
                     new Product
                     {
                         Name = "Tortoise Hoop Stype",
                         Sku = $"PE{_rand.Next(x, 500)}",
                         ShortDescription = "The Aria Earring is a timeless tortoise and gold-tone hoop earring.",
                         FullDescription = """
                         Simple and sophisticated, our ultra lightweight Tortoise Hoops easily match your favorite cutoff jeans or your little black dress. These earrings are so versatile and comfortable, they may become your new go-to pair as they are Simple enough for every day wear but elegant enough wear with your favorite black dress.
                         """,
                         Category = "Earrings",
                         Price = 29.95m,
                         Images = [new Image {
                             IsMainImage = true,
                             ImageUrl = "https://images.jane.com/sellers/funky-monkey-fashion-accessories/product-images/AriaTortiseHoop.jpg?preset=xlarge"
                         }]
                     },
                     new Product
                     {
                         Name = "Glitter Stud Drop Earrings",
                         Sku = $"PE{_rand.Next(x, 500)}",
                         ShortDescription = "Handcrafted in USA for sensitive ears.",
                         FullDescription = """
                         Handcrafted in USA for sensitive ears.
                         Size: 1.75"
                         Material: Premium Glitter on leather. Wood connector, surgical steel posts & tarnish free.
                         Large secure rubber backs included.
                         """,
                         Category = "Earrings",
                         Price = 8.99m,
                         Images = [new Image {
                             IsMainImage = true,
                             ImageUrl = "https://images.jane.com/sellers/mrs-little-shoppe-llc/product-images/Bazaart_20241008_050716_901.jpeg?preset=xlarge"
                         }]
                     },
                     new Product
                     {
                         Name = "Samantha Peach and Blush Beaded Hoop Earrings",
                         Sku = $"PE{_rand.Next(x, 500)}",
                         ShortDescription = "The Samantha Peach and Blush Beaded Hoop Earrings",
                         FullDescription = "The Samantha Peach and Blush Beaded Hoop Earrings showcase a delicate balance of sophistication and femininity, featuring blush-colored beads elegantly intertwined with gold hoops, making them a graceful addition to any ensemble.",
                         Category = "Earrings",
                         Price = 16.99m,
                         Images = [new Image {
                             IsMainImage = true,
                             ImageUrl = "https://images.jane.com/sellers/shop-suey-boutique/product-images/9083056292119.jpg?preset=xlarge"
                         }]
                     }
                }));

                context.SaveChanges();
            }
        }

        public static void SaveProductWithImages(ApplicationDbContext context, Image image, Product p)
        {

        }
    }
}

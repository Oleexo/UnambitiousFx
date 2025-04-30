# UnambitiousFx Documentation Site

This directory contains the Jekyll-based documentation site for UnambitiousFx. The site is designed to provide information about the project and detailed documentation for each library.

## Site Structure

- `_config.yml` - Main Jekyll configuration file
- `_data/` - Data files for navigation and other site data
- `_docs/` - Documentation files for each library
- `_layouts/` - Custom layout templates
- `_posts/` - Blog posts (if any)
- `assets/` - CSS, JavaScript, and image files
- `index.markdown` - Home page
- `about.markdown` - About page
- `docs.markdown` - Documentation landing page

## Running Locally

To run the site locally, you'll need to have Ruby and Jekyll installed. Follow these steps:

1. Install Ruby (if not already installed)
2. Install Jekyll and Bundler:
   ```
   gem install jekyll bundler
   ```
3. Navigate to the docs directory:
   ```
   cd docs
   ```
4. Install dependencies:
   ```
   bundle install
   ```
5. Start the local server:
   ```
   bundle exec jekyll serve
   ```
6. Open your browser and go to `http://localhost:4000`

## Adding Content

### Adding a New Library

To add documentation for a new library:

1. Create a new Markdown file in the `_docs` directory, e.g., `_docs/new-library.md`
2. Add the following front matter:
   ```yaml
   ---
   layout: docs
   title: UnambitiousFx.NewLibrary
   nav_order: 4  # Adjust as needed
   ---
   ```
3. Add the library documentation content
4. Update the navigation in `_data/navigation.yml` to include the new library

### Updating Existing Documentation

To update existing documentation:

1. Find the appropriate Markdown file in the `_docs` directory
2. Edit the content as needed
3. If you're adding new sections, consider updating the navigation in `_data/navigation.yml`

## Customizing the Site

### Changing the Theme

The site uses the Minima theme with custom styles. To change the theme:

1. Update the `theme` setting in `_config.yml`
2. Install the new theme gem if needed
3. Update any custom styles in `assets/main.scss`

### Modifying Navigation

The site navigation is defined in `_data/navigation.yml`. To modify the navigation:

1. Edit the appropriate section in `_data/navigation.yml`
2. For main navigation, update the `main` section
3. For documentation navigation, update the `docs` section

### Adding Custom Styles

Custom styles are defined in `assets/main.scss`. To add or modify styles:

1. Edit `assets/main.scss`
2. Add your custom CSS/SCSS
3. Make sure to keep the front matter at the top of the file

### Dark Theme

The site includes a dark theme with a toggle button in the header. The theme implementation uses:

1. CSS variables for consistent theming across light and dark modes
2. JavaScript for theme switching and saving user preferences
3. A toggle switch in the header for easy switching

To customize the dark theme:

1. Edit the CSS variables in `_includes/head.html` under the `[data-theme="dark"]` section
2. Adjust colors, backgrounds, and other theme properties as needed
3. Test both light and dark themes to ensure good contrast and readability

## Deployment

The site can be deployed to GitHub Pages or any other static site hosting service. To build the site for deployment:

```
bundle exec jekyll build
```

This will generate the static site files in the `_site` directory, which can then be deployed to your hosting service.

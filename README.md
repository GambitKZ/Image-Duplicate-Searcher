# Image Duplicate Searcher

## Description

Image Duplicate Searcher is a .NET console application designed to identify duplicate images within a specified directory. It uses perceptual hashing to compare images, making it effective at detecting visually similar images even if they have different file names or minor modifications.

The application processes images by generating thumbnails, computing perceptual hashes, and grouping images with identical hashes. Results are displayed in the console and saved to a JSON file for further analysis.

## Features

- **Perceptual Hashing**: Uses advanced hashing to detect visually identical images.
- **Supported Formats**: JPEG, JPG, PNG, BMP.
- **Console Output**: Displays duplicate groups with file paths and sizes.
- **JSON Export**: Saves detailed duplicate information to a configurable output file.
- **Configurable**: Easily adjust scan directory, thumbnail size, output path, and supported formats via configuration.

## Requirements

- .NET 10.0 or later
- Windows (as per the current environment)

## Installation

1. Ensure you have .NET 10.0 installed.
2. Clone or download the project to your local machine.
3. Navigate to the `src` directory.

## Usage

1. **Configure the Application**:
   - Open `appsettings.json` in the `src` directory.
   - Update the settings as needed:
     ```json
     {
       "ImageDuplicationOptions": {
         "ImageDirectory": "./images",  // Path to the directory containing images
         "OutputFilePath": "duplicates.json",  // Path for the output JSON file
         "SupportedFormats": [".jpeg", ".jpg", ".png", ".bmp"]  // Supported image formats
       }
     }
     ```

2. **Run the Application**:
   - From the `src` directory, execute:
     ```
     dotnet run
     ```
   - The application will scan the specified directory, process images, and display progress.
   - Duplicate groups will be shown in the console.
   - Results will be saved to the specified output file.

## How It Works

1. **Image Scanning**: The application scans the configured directory for files matching the supported formats.
2. **Thumbnail Generation**: For each image, a thumbnail is created to standardize comparison.
3. **Hash Computation**: A perceptual hash is computed for each thumbnail.
4. **Grouping**: Images with identical hashes are grouped as duplicates.
5. **Reporting**: Duplicates are reported to the console and exported to JSON.

## Configuration Options

- `ImageDirectory`: The path to the directory to scan for images.
- `ThumbnailSize`: The size of the square thumbnail in pixels (used for hashing).
- `OutputFilePath`: The file path where duplicate results will be saved in JSON format.
- `SupportedFormats`: A list of file extensions to consider as images.

## Output

- **Console**: Displays the number of images found, progress bar during processing, and lists of duplicate groups with file paths and sizes.
- **JSON File**: Contains an array of duplicate groups, each with a hash and list of images (path and size in MB).

Example JSON output:
```json
[
  {
    "hash": 123456789,
    "images": [
      {
        "path": "path/to/image1.jpg",
        "sizeMB": 2.5
      },
      {
        "path": "path/to/image2.jpg",
        "sizeMB": 2.5
      }
    ]
  }
]
```

## Troubleshooting

- Ensure the `ImageDirectory` path exists and contains images.
- Check that .NET 10.0 is installed and accessible.
- Verify supported formats match your image files.


## License

This project is licensed under the MIT License.
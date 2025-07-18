package org.example;

class DirectoryResult {
    String dirPath;
    long size; // rozmiar w bajtach
    long processingTimeMillis; // czas przetwarzania

    public DirectoryResult(String dirPath, long size, long processingTimeMillis) {
        this.dirPath = dirPath;
        this.size = size;
        this.processingTimeMillis = processingTimeMillis;
    }

    @Override
    public String toString() {
        return String.format("Katalog: %s, Rozmiar: %d KB, Czas: %d ms",
                dirPath, size / 1024, processingTimeMillis);
    }
}
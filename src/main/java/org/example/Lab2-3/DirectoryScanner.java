package org.example;

import java.io.File;
import java.util.concurrent.TimeUnit;

import static org.example.Main.*;

class DirectoryScanner implements Runnable {
    @Override
    public void run() {
        while (running.get() || !dirQueue.isEmpty()) {
            File dir = null;
            try {
                // Pobierz katalog z kolejki – czekamy maksymalnie 1 sekundę, by umożliwić zakończenie.
                dir = dirQueue.poll(1, TimeUnit.SECONDS);
            } catch (InterruptedException e) {
                //System.out.println("BLOK");
                Thread.currentThread().interrupt();
            }
            if (dir == null) {
                break;
            }

            long startTime = System.currentTimeMillis();
            long size = calculateDirectorySize(dir);
            long endTime = System.currentTimeMillis();
            long processingTime = endTime - startTime;

            // Aktualizujemy globalny rozmiar – dodajemy wynik przetwarzania tego katalogu.
            totalSize.addAndGet(size);

            // Zapisujemy wynik obliczeń w współdzielonym zasobie.
            DirectoryResult dr = new DirectoryResult(dir.getAbsolutePath(), size, processingTime);
            resultList.add(dr);

            System.out.printf("Przetworzono katalog: %s, Rozmiar: %f MB, Czas: %d ms%n",
                    dr.dirPath, size / 1024.0 / 1024.0, processingTime);

            // Dodajemy znalezione podkatalogi do kolejki.

            File[] files = dir.listFiles();
            if (files != null) {
                for (File f : files) {
                    if (f.isDirectory()) {
                        // Upewniamy się, że katalog jest czytelny
                        if (f.canRead()) {
                            dirQueue.offer(f);
                        }
                    }
                }
            }

        }
        System.out.println("Wątek " + Thread.currentThread().getName() + " zakończył pracę.");

    }

    // Metoda obliczająca rozmiar katalogu – sumuje rozmiary plików w katalogu (bez rekurencji w podkatalogach).
    // Możesz rozszerzyć rekurencyjne przeszukiwanie, jeśli chcesz.
    private long calculateDirectorySize(File dir) {
        long size = 0;
        File[] files = dir.listFiles();
        if (files == null) return 0;
        for (File f : files) {
            if (f.isFile() && f.canRead()) {
                size += f.length();
            }
        }
        return size;
    }
}
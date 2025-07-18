package org.example;

import java.io.File;
import java.util.*;
import java.util.concurrent.*;
import java.util.concurrent.atomic.*;
import java.util.Scanner;

public class Main {

    // Zadanie 1: Współdzielony zasób – kolejka katalogów do przetworzenia.
    public static BlockingQueue<File> dirQueue = new LinkedBlockingQueue<>();

    // Zadanie 3: Współdzielony zasób wyników – zapamiętujemy wyniki obliczeń dla poszczególnych katalogów.
    // Klucz: ścieżka katalogu, wartość: wynik obliczeń (rozmiar) oraz czas przetwarzania.
    public static List<DirectoryResult> resultList = Collections.synchronizedList(new ArrayList<>());

    // Współdzielony parametr – aktualnie obliczony łączny rozmiar danych.
    public static AtomicLong totalSize = new AtomicLong(0);

    // Flaga sterująca działaniem aplikacji
    public static AtomicBoolean running = new AtomicBoolean(true);

    // Wynik obliczeń dla pojedynczego katalogu.


    // Zadanie 2: Wątek skanujący katalogi – pobiera katalog z kolejki, oblicza jego rozmiar oraz
    // dodaje nowe katalogi do kolejki.


    public static void main(String[] args) {
        double startTime = System.currentTimeMillis();
        // Parametr określający liczbę wątków – domyślnie 4, można zmienić przez parametr startowy.
        int numThreads = 4;
        if (args.length > 0) {
            try {
                numThreads = Integer.parseInt(args[0]);
            } catch (NumberFormatException e) {
                System.out.println("Niepoprawna liczba wątków, używam domyślnej wartości 4.");
            }
        }

        // Zadanie 1: Dodajemy katalog startowy do kolejki. Zmień ścieżkę na odpowiednią.
        File startDir = new File("C:\\Windows");  // lub inna ścieżka np. "/home/user"
        if (!startDir.exists() || !startDir.isDirectory()) {
            System.out.println("Niepoprawna ścieżka startowa!");
            System.exit(1);
        }
        dirQueue.offer(startDir);

        // Uruchamiamy ExecutorService, który obsłuży wątki skanujące.
        ExecutorService executor = Executors.newFixedThreadPool(numThreads);
        for (int i = 0; i < numThreads; i++) {
            executor.execute(new DirectoryScanner());

        }

        // Zadanie 4: Uruchomienie wątku monitorującego wejście użytkownika
        // Wątek monitorujący wejście użytkownika.
        // Jeśli użytkownik wpisze "exit", ustawiamy running na false i wywołujemy shutdownNow().
        Thread userInputThread = new Thread(() -> {
            Scanner scanner = new Scanner(System.in);
            System.out.println("Wpisz 'exit' aby natychmiast zakończyć aplikację:");
            try {
                String input = scanner.nextLine();
                if ("exit".equalsIgnoreCase(input.trim())) {
                    System.out.println("Polecenie 'exit' otrzymane. Kończenie pracy...");
                    running.set(false);
                    executor.shutdownNow(); // Przerywamy działanie wątków.
                }
            } finally {
                scanner.close();
            }
        }, "UserInputThread");
        userInputThread.setDaemon(true);
        userInputThread.start();

        // W głównym wątku czekamy na zakończenie pracy wszystkich wątków skanujących.
        executor.shutdown();
        try {
            if (!executor.awaitTermination(75, TimeUnit.SECONDS)) {
                executor.shutdownNow();
                running.set(false);
            }
        } catch (InterruptedException e) {
            executor.shutdownNow();
            running.set(false);
        }

        // Jeśli zadania zakończyły się wcześniej, przerywamy wątek wejścia użytkownika.
        /*
        if (userInputThread.isAlive()) {
            userInputThread.interrupt();
        }
        */
        // Wyświetlamy końcowy wynik – całkowity rozmiar danych
        System.out.printf("Całkowity rozmiar danych: %,.2f GB%n", totalSize.get()/ 1024.0 / 1024.0 / 1024.0);
        /*System.out.println("Wyniki poszczególnych katalogów:");
        for (DirectoryResult dr : resultList) {
            System.out.println(dr);
        }*/
        double endTime = System.currentTimeMillis();
        double elapsedTime = (endTime - startTime) / 1000;
        System.out.printf("Czas działania aplikacji: %,.2fs\n", elapsedTime);
        System.out.println("Aplikacja zakończyła działanie.");
        return;
    }
}
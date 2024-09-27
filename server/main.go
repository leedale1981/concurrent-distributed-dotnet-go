package main

import (
	"bufio"
	"fmt"
	"log"
	"math/rand"
	"net"
	"os"
	"time"
)

func main() {
	port := os.Args[1]

	listener, err := net.Listen("tcp", fmt.Sprintf("localhost:%v", port))

	if err != nil {
		log.Fatal(err)
	}

	for {
		conn, err := listener.Accept()
		if err != nil {
			log.Print(err)
			continue
		}
		// Concurrent handling of each connection
		go handleConnection(conn)
	}
}

func handleConnection(c net.Conn) {
	defer c.Close()

	minTemp := 2
	maxTemp := 50
	temperature := rand.Intn(maxTemp-minTemp) + minTemp

	minSleep := 1
	maxSleep := 5
	sleep := rand.Intn(maxSleep-minSleep) + minSleep
	time.Sleep(time.Duration(sleep) * time.Second)

	rw := bufio.NewReadWriter(bufio.NewReader(c), bufio.NewWriter(c))
	rw.WriteString(string(temperature))
	rw.Flush()
	log.Printf("Sending temperature %v", temperature)
}
